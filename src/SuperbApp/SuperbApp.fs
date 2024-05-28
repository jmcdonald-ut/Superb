namespace SuperbApp

type Query() =
  member _.GetTcpListeners() =
    match TcpListeners.all () with
    | Ok list -> list |> List.map Schemata.TcpListenerType |> List.toArray
    | Error _ -> [||]

  member _.GetHackerNewsStories() =
    Async.RunSynchronously(News.loadTopStories ())

type Mutation() =
  member _.ExecuteRedisCLICommand(command: string) =
    match RedisCLI.send command with
    | Ok output -> output.Trim()
    | Error reason -> sprintf "ERROR: %s" (reason.Trim())
