namespace SuperbApp

open HotChocolate
open HotChocolate.Types

open SuperbApp.Features
open SuperbApp.Features.MySQL

module Schemata =
  type TcpListenerType(tcpListener: TcpListener) =
    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.ProcessId = tcpListener.ProcessId

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.Command = tcpListener.Command

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.User = tcpListener.User

    [<GraphQLType(typeof<NonNullType<ListType<NonNullType<StringType>>>>)>]
    member _.Hosts = tcpListener.Hosts

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

  type SchemaType(schema: MySQL.Schema) =
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

  type TableType(table: MySQL.Table) =
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

  type RowFieldValueType(row: MySQL.RowFieldValue) =
    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.Key = row.Key

    // TODO: The non-null constraint is true right now, but I don't want to
    // send empty string for null values. This will likely need to change.
    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.Value = row.Value

  // TODO: This will also include a Columns field which provides metadata for
  // each of the table's columns. Or maybe I'll add that to the TableType and
  // embed that here. Not sure to be honest.
  type RowType(row: MySQL.Row) =
    member _.Values = List.map RowFieldValueType row.Values
