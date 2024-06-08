namespace SuperbApp

open HotChocolate
open HotChocolate.Types

open SuperbApp.Features
open SuperbApp.Schemata

type Query() =
  member _.GetSchemata() =
    MySQL.querySchemas |> Seq.map SchemaType |> Seq.toArray

  member _.GetTables([<GraphQLType(typeof<NonNullType<StringType>>)>] schemaName: string) =
    schemaName |> MySQL.queryTables |> Seq.map TableType |> Seq.toArray

  member _.GetTcpListeners() =
    match TcpListeners.all () with
    | Ok list -> list |> List.map TcpListenerType |> List.toArray
    | Error _ -> [||]

  member _.GetHackerNewsStories() : StoryType array =
    News.loadTopStories ()
    |> Async.RunSynchronously
    |> List.map StoryType
    |> List.toArray

type Mutation() =
  member _.ExecuteRedisCLICommand(command: string) =
    match RedisCLI.send command with
    | Ok output -> output.Trim()
    | Error reason -> sprintf "ERROR: %s" (reason.Trim())
