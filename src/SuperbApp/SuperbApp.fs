namespace SuperbApp

type Query() =
  member _.GetTcpListeners() =
    match TcpListeners.all () with
    | Ok list -> list |> List.map Schemata.TcpListenerType |> List.toArray
    | Error _ -> [||]

  member _.GetHackerNewsStories() =
    Async.RunSynchronously(News.loadTopStories ())
