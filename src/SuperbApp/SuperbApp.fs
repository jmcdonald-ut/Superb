namespace SuperbApp

open HotChocolate
open HotChocolate.Types

type Query() =
  member _.GetSchemata() =
    Schemata.querySchemas |> Seq.map Schemata.SchemaType |> Seq.toArray

  member _.GetTables([<GraphQLType(typeof<NonNullType<StringType>>)>] schemaName: string) =
    schemaName |> Schemata.queryTables |> Seq.map Schemata.TableType |> Seq.toArray

  member _.GetTcpListeners() =
    match TcpListeners.all () with
    | Ok list -> list |> List.map Schemata.TcpListenerType |> List.toArray
    | Error _ -> [||]

  member _.GetHackerNewsStories() : Schemata.StoryType array =
    Async.RunSynchronously(News.loadTopStories ())

type Mutation() =
  member _.ExecuteRedisCLICommand(command: string) =
    match RedisCLI.send command with
    | Ok output -> output.Trim()
    | Error reason -> sprintf "ERROR: %s" (reason.Trim())
