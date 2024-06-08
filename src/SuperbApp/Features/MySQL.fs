namespace SuperbApp.Features

open FSharp.Data.LiteralProviders
open FSharp.Data.Sql
open System.Linq

/// <summary>
/// Explore and detail the schemas in the configured database, the tables
/// within those schemas, and the data within those tables. Some of these
/// functions may introduce security risks. Potential security risks are
/// covered in the remarks.
///
/// <see cref="SuperbApp.Features.MySQL.getListOfAllSchemas" /> retrieves a
/// list of <see cref="SuperbApp.Features.MySQL.Schema" /> records. An
/// alternative function, <see cref="SuperbApp.Features.MySQL.querySchemas" />
/// can be used to get a queryable that will yield schema records.
///
/// <see cref="SuperbApp.Features.MySQL.getListOfAllTables" /> retrieves a
/// list of <see cref="SuperbApp.Features.MySQL.Table" /> records. For a
/// queryable, use <see cref="SuperbApp.Features.MySQL.queryTables" />.
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

  let private ctx = sql.GetDataContext()

  /// <summary>
  /// Queryable fetching all schemas
  /// </summary>
  let querySchemas () =
    query {
      for row in ctx.InformationSchema.Schemata do
        sortBy (row.SchemaName)
        select row
    }

  /// Retrieves a plain list of all schemas visible based on the configured
  /// connection string.
  let getListOfAllSchemas () = querySchemas () |> Seq.toList

  /// Queryable which yields all visible tables within the given schema.
  let queryTables (schemaName: string) : IQueryable<Table> =
    query {
      for row in ctx.InformationSchema.Tables do
        where (row.TableSchema = schemaName)
        sortBy (row.TableName)
        select row
    }

  /// Retrieves all visible tables within the given schema.
  let getListOfAllTablesInSchema (schemaName: string) = schemaName |> queryTables |> Seq.toList
