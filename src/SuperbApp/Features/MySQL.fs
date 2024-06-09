namespace SuperbApp.Features

open FSharp.Data.LiteralProviders
open FSharp.Data.Sql
open MySql.Data.MySqlClient
open System.Data
open System.Data.SqlClient
open System.Linq

/// <summary>
/// Explore and detail the schemas in the configured database, the tables
/// within those schemas, and the data within those tables. Some of these
/// functions may introduce security risks. Potential security risks are
/// covered in the remarks.
///
/// <see cref="SuperbApp.Features.MySQL.getListOfAllSchemas" /> retrieves a
/// list of <see cref="SuperbApp.Features.MySQL.Schema" /> records. An
/// alternative function, <see cref="SuperbApp.Features.MySQL.allSchemasQuery" />
/// can be used to get a queryable that will yield schema records.
///
/// <see cref="SuperbApp.Features.MySQL.getListOfAllTables" /> retrieves a
/// list of <see cref="SuperbApp.Features.MySQL.Table" /> records. For a
/// queryable, use <see cref="SuperbApp.Features.MySQL.allTablesQuery" />.
///
/// Functions for getting rows from a table are forthcoming.
/// </summary>
/// <remarks>
/// There may be potential security risks involved with using the
/// functionality offered here. Superb is only meant to be run locally, in a
/// secure environment. The connection string should be configured to only
/// provide read access to relevant schemas and data.
///
/// It's important to note that the query for retrieving table rows is built
/// dynamically, with input from the client. The logic will verify that the
/// given schema and table are both visible based on the configured connection
/// string. Still, use with caution.
/// </remarks>
module MySQL =
  [<Literal>]
  let private connectionStr =
    Env<"SUPERB_MYSQL_CONNECTION_STRING", "server=HOST;port=3306;uid=USER;pwd=PASSWORD;database=DB">.Value

  [<Literal>]
  let private dbVendor = Common.DatabaseProviderTypes.MYSQL

  type private sql = SqlDataProvider<ConnectionString=connectionStr, DatabaseVendor=dbVendor>
  type Table = sql.dataContext.``information_schema.TABLESEntity``
  type Schema = sql.dataContext.``information_schema.SCHEMATAEntity``
  type RowFieldValue = { Key: string; Value: string }
  type Row = { Values: RowFieldValue list }

  let private ctx = sql.GetDataContext()

  /// Queryable fetching all schemas
  let allSchemasQuery () =
    query {
      for row in ctx.InformationSchema.Schemata do
        sortBy (row.SchemaName)
        select row
    }

  /// Queryable which yields all visible tables within the given schema.
  let allTablesQuery (schemaName: string) : IQueryable<Table> =
    query {
      for row in ctx.InformationSchema.Tables do
        where (row.TableSchema = schemaName)
        sortBy (row.TableName)
        select row
    }

  /// Retrieves a single table scoped by schema and table name.
  let oneTableByNameAndSchema (schemaName: string) (tableName: string) : Table =
    query {
      for table in allTablesQuery (schemaName) do
        where (table.TableName = tableName)
        select table
        headOrDefault
    }

  /// Retrieves a plain list of all schemas visible based on the configured
  /// connection string.
  let getListOfAllSchemas () = allSchemasQuery () |> Seq.toList

  /// Retrieves all visible tables within the given schema.
  let getListOfAllTablesInSchema (schemaName: string) =
    schemaName |> allTablesQuery |> Seq.toList

  /// Retrieves an array of table rows that is up to `takeCount` in length.
  /// This function is provided for use from the client side (via GraphQL). The
  /// schema and table name ultimately come from there.
  let dangerouslyTakeTableRowsDynamically (takeCount: int) (schemaName: string) (tableName: string) =
    // TODO: Error handling if this query has no result.
    // TODO: Explicit ORDER BY with primary key. Primary key info is available
    //   in information_schema.columns.
    let targetTable = oneTableByNameAndSchema schemaName tableName
    let queryString = sprintf "SELECT * FROM %s LIMIT @limit" targetTable.TableName

    // Since we're accepting schema name and table name dynamically, we can't
    // use nicely typed queries with LINQ. The monstrosity that follows is
    // primarily to work around that and fetch data.
    try
      // Use ENV['SUPERB_MYSQL_CONNECTION_STRING'] to build the base connection
      // string. Then explicitly override the DB name so we connect to what the
      // caller gave us.
      let connectionStringBuilder = SqlConnectionStringBuilder(connectionStr)
      connectionStringBuilder["database"] <- schemaName

      // "use" is critical here. This is what cleans up the MySQL connection
      // once we exit this block.
      use connection = new MySqlConnection(connectionStringBuilder.ConnectionString)
      connection.Open()

      use command = new MySqlCommand(queryString, connection)
      command.Parameters.Add(MySqlParameter("@limit", SqlDbType.Int)).Value <- takeCount
      let reader = command.ExecuteReader()

      // There's probably a cleaner way to do this, but my brain is fried. Init
      // a mutable value and populate while we read through each row.
      let mutable mutList: Row list = []

      while reader.Read() do
        // https://stackoverflow.com/a/4286071
        let enum =
          Enumerable
            .Range(0, reader.FieldCount)
            .ToDictionary(reader.GetName, reader.GetValue)
            .Select(fun kvp -> {
              Key = kvp.Key
              Value = kvp.Value.ToString()
            })

        mutList <- { Values = enum |> Seq.toList } :: mutList

      List.rev mutList
    with err ->
      // TODO: Real error handling, of course.
      System.Console.WriteLine(sprintf "OH NO FAILURES! %s %A" (err.ToString()) err)
      []
