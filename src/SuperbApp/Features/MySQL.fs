namespace SuperbApp.Features

open FSharp.Data.LiteralProviders
open FSharp.Data.Sql
open System.Linq

module MySQL =
  [<Literal>]
  let connectionStr =
    Env<"SUPERB_MYSQL_CONNECTION_STRING", "server=HOST;port=3306;uid=USER;pwd=PASSWORD;database=DB">.Value

  [<Literal>]
  let dbVendor = Common.DatabaseProviderTypes.MYSQL

  type sql = SqlDataProvider<ConnectionString=connectionStr, DatabaseVendor=dbVendor>
  type Table = sql.dataContext.``information_schema.TABLESEntity``
  type Schema = sql.dataContext.``information_schema.SCHEMATAEntity``

  let ctx = sql.GetDataContext()

  let queryTables (schemaName: string) : IQueryable<Table> =
    query {
      for row in ctx.InformationSchema.Tables do
        where (row.TableSchema = schemaName)
        sortBy (row.TableName)
        select row
    }

  let querySchemas =
    query {
      for row in ctx.InformationSchema.Schemata do
        sortBy (row.SchemaName)
        select row
    }
