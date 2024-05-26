namespace SuperbUi

open Feliz

type Containers =
  /// <summary>
  /// A superb dashboard ;)
  /// </summary>
  [<ReactComponent>]
  static member DashboardContainer() =
    let (tcpListeners, setTcpListeners) =
      React.useState<GraphQLClient.TcpListener list> ([])

    let getTcpListeners () =
      async {
        match! GraphQLClient.getTcpListeners () with
        | Ok tcpListeners -> tcpListeners |> setTcpListeners |> ignore
        | Error _ -> ()
      }

    React.useEffect (getTcpListeners >> Async.StartImmediate, [||])

    Html.div [ DashboardComponents.TcpListeners(tcpListeners = tcpListeners) ]
