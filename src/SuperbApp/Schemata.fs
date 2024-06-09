namespace SuperbApp

open HotChocolate
open HotChocolate.Types

open SuperbApp.Features
open SuperbApp.Features.MySQL

type private Required<'Field when 'Field :> IType> = NonNullType<'Field>
type private RequiredString = Required<StringType>
type private RequiredList<'Field when 'Field :> IType> = NonNullType<ListType<'Field>>

module Schemata =
  type TcpListenerType(tcpListener: TcpListener) =
    [<GraphQLType(typeof<RequiredString>)>]
    member _.ProcessId = tcpListener.ProcessId

    [<GraphQLType(typeof<RequiredString>)>]
    member _.Command = tcpListener.Command

    [<GraphQLType(typeof<RequiredString>)>]
    member _.User = tcpListener.User

    [<GraphQLType(typeof<RequiredList<RequiredString>>)>]
    member _.Hosts = tcpListener.Hosts

  type StoryType(story: Story) =
    member _.StoryId = story.StoryId
    member _.CommentCount = story.CommentCount

    [<GraphQLNonNullType>]
    member _.Comments = story.Comments

    [<GraphQLType(typeof<RequiredString>)>]
    member _.By = story.By

    [<GraphQLType(typeof<RequiredString>)>]
    member _.Url = story.Url

    [<GraphQLType(typeof<RequiredString>)>]
    member _.Title = story.Title

  type SchemaType(schema: MySQL.Schema) =
    [<GraphQLType(typeof<RequiredString>)>]
    member _.CatalogName = schema.CatalogName

    [<GraphQLType(typeof<RequiredString>)>]
    member _.DefaultCharacterSetName = schema.DefaultCharacterSetName

    [<GraphQLType(typeof<RequiredString>)>]
    member _.DefaultCollationName = schema.DefaultCollationName

    [<GraphQLType(typeof<RequiredString>)>]
    member _.DefaultEncryption = schema.DefaultEncryption

    [<GraphQLType(typeof<RequiredString>)>]
    member _.SchemaName = schema.SchemaName

  type TableType(table: MySQL.Table) =
    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.AutoIncrement = table.AutoIncrement

    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.AvgRowLength = table.AvgRowLength

    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.Checksum = table.Checksum

    member _.CheckTime = table.CheckTime

    [<GraphQLType(typeof<RequiredString>)>]
    member _.CreateOptions = table.CreateOptions

    member _.CreateTime = table.CreateTime

    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.DataFree = table.DataFree

    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.DataLength = table.DataLength

    [<GraphQLType(typeof<RequiredString>)>]
    member _.Engine = table.Engine

    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.IndexLength = table.IndexLength

    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.MaxDataLength = table.MaxDataLength

    [<GraphQLType(typeof<RequiredString>)>]
    member _.RowFormat = table.RowFormat

    [<GraphQLType(typeof<RequiredString>)>]
    member _.TableCatalog = table.TableCatalog

    [<GraphQLType(typeof<RequiredString>)>]
    member _.TableCollation = table.TableCollation

    [<GraphQLType(typeof<RequiredString>)>]
    member _.TableComment = table.TableComment

    [<GraphQLType(typeof<RequiredString>)>]
    member _.TableName = table.TableName

    [<GraphQLType(typeof<NonNullType<IntType>>)>]
    member _.TableRows = table.TableRows

    [<GraphQLType(typeof<RequiredString>)>]
    member _.TableSchema = table.TableSchema

    [<GraphQLType(typeof<RequiredString>)>]
    member _.TableType = table.TableType

    member _.UpdateTime = table.UpdateTime

    [<GraphQLType(typeof<IntType>)>]
    member _.Version = table.Version

  type RowFieldValueType(row: MySQL.RowFieldValue) =
    [<GraphQLType(typeof<RequiredString>)>]
    member _.Key = row.Key

    // TODO: The non-null constraint is true right now, but I don't want to
    // send empty string for null values. This will likely need to change.
    [<GraphQLType(typeof<RequiredString>)>]
    member _.Value = row.Value

  type ColumnType(column: MySQL.TableColumn) =
    // TODO: Support more fields. Just rolling with this for now in the
    //   interest of getting a MVP built out.
    [<GraphQLType(typeof<RequiredString>)>]
    member _.ColumnComment = column.ColumnComment

    [<GraphQLType(typeof<RequiredString>)>]
    member _.ColumnDefault = column.ColumnDefault

    [<GraphQLType(typeof<RequiredString>)>]
    member _.ColumnKey = column.ColumnKey

    [<GraphQLType(typeof<RequiredString>)>]
    member _.ColumnName = column.ColumnName

    [<GraphQLType(typeof<RequiredString>)>]
    member _.DataType = column.DataType

    [<GraphQLType(typeof<RequiredString>)>]
    member _.CharacterMaximumLength = column.CharacterMaximumLength

    [<GraphQLType(typeof<Required<IntType>>)>]
    member _.OrdinalPosition = column.OrdinalPosition

  type RowType(row: MySQL.Row) =
    [<GraphQLType(typeof<RequiredList<Required<ObjectType<RowFieldValueType>>>>)>]
    member _.Values = List.map RowFieldValueType row.Values

  type SampleOfTableRowsType(rows: MySQL.Row seq, columns: MySQL.TableColumn seq) =
    [<GraphQLType(typeof<RequiredList<Required<ObjectType<ColumnType>>>>)>]
    member _.Columns = columns |> Seq.map ColumnType |> Seq.toList

    [<GraphQLType(typeof<RequiredList<Required<ObjectType<RowType>>>>)>]
    member _.Rows = rows |> Seq.map RowType |> Seq.toList
