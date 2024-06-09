namespace SuperbApp

open HotChocolate
open HotChocolate.Types

open SuperbApp.Features
open SuperbApp.Schemata

/// The root query type intended for use with GraphQL. Each member represents
/// an available root field.
type Query() =
  member _.GetSchemata() =
    MySQL.getListOfAllSchemas () |> Seq.map SchemaType |> Seq.toArray

  member _.GetTables([<GraphQLType(typeof<NonNullType<StringType>>)>] schemaName: string) =
    schemaName
    |> MySQL.getListOfAllTablesInSchema
    |> Seq.map TableType
    |> Seq.toArray

  member _.GetTableRows
    (
      [<GraphQLType(typeof<NonNullType<StringType>>)>] schemaName: string,
      [<GraphQLType(typeof<NonNullType<StringType>>)>] tableName: string,
      [<GraphQLType(typeof<NonNullType<IntType>>)>] count: int
    ) =
    tableName
    |> MySQL.dangerouslyTakeTableRowsDynamically count schemaName
    |> List.map RowType

  member _.GetTcpListeners() =
    match TcpListeners.all () with
    | Ok list -> list |> List.map TcpListenerType |> List.toArray
    | Error _ -> [||]

  member _.GetHackerNewsStories() : StoryType array =
    News.loadTopStories ()
    |> Async.RunSynchronously
    |> List.map StoryType
    |> List.toArray

/// Root mutation type intended for use with GraphQL. Each member represents
/// a mutation.
type Mutation() =
  member _.ExecuteRedisCLICommand(command: string) =
    match RedisCLI.send command with
    | Ok output -> output.Trim()
    | Error reason -> sprintf "ERROR: %s" (reason.Trim())
