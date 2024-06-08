namespace SuperbApp

open FSharp.Data.LiteralProviders
open FSharp.Data.Sql
open HotChocolate
open HotChocolate.Types
open System.Linq

module Schemata =
  type TcpListener = {
    ProcessId: string
    Command: string
    User: string
    Hosts: string list
  } with

    static member Default = {
      ProcessId = "<UNKNOWN>"
      Command = "<UNKNOWN>"
      User = "<UNKNOWN>"
      Hosts = []
    }

  type TcpListenerType(tcpListener: TcpListener) =
    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.ProcessId = tcpListener.ProcessId

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.Command = tcpListener.Command

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.User = tcpListener.User

    [<GraphQLType(typeof<NonNullType<ListType<NonNullType<StringType>>>>)>]
    member _.Hosts = tcpListener.Hosts

  type Id = int
  type Author = string
  type Url = string
  type Title = string
  type Kids = int array

  type Story = {
    StoryId: Id
    By: Author
    Url: Url
    Title: Title
    Comments: Kids
    CommentCount: int
  }

  type StoryType(story: Story) =
    member _.StoryId = story.StoryId
    member _.CommentCount = story.CommentCount

    [<GraphQLNonNullType>]
    member _.Comments = story.Comments

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.By = story.By

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.Url = story.Url

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.Title = story.Title

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

  type SchemaType(schema: Schema) =
    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.CatalogName = schema.CatalogName

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.DefaultCharacterSetName = schema.DefaultCharacterSetName

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.DefaultCollationName = schema.DefaultCollationName

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.DefaultEncryption = schema.DefaultEncryption

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.SchemaName = schema.SchemaName

  type TableType(table: Table) =
    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.AutoIncrement = table.AutoIncrement

    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.AvgRowLength = table.AvgRowLength

    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.Checksum = table.Checksum

    member _.CheckTime = table.CheckTime

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.CreateOptions = table.CreateOptions

    member _.CreateTime = table.CreateTime

    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.DataFree = table.DataFree

    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.DataLength = table.DataLength

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.Engine = table.Engine

    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.IndexLength = table.IndexLength

    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.MaxDataLength = table.MaxDataLength

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.RowFormat = table.RowFormat

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.TableCatalog = table.TableCatalog

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.TableCollation = table.TableCollation

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.TableComment = table.TableComment

    // GO
    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.TableName = table.TableName

    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.TableRows = table.TableRows

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.TableSchema = table.TableSchema

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.TableType = table.TableType

    member _.UpdateTime = table.UpdateTime

    [<GraphQLType(typeof<IntType>)>]
    member _.Version = table.Version
