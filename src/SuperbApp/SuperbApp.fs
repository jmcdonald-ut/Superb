namespace SuperbApp

type Query() =
  member this.GetTcpListeners() =
    match TcpListeners.all () with
    | Ok list -> list |> List.toArray
    | Error _ -> [||]
