namespace SuperbUi

open Feliz
open Feliz.DaisyUI

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

    Html.div [
      theme.nord
      prop.className "container mx-auto grid grid-cols-2 py-4"
      prop.children [ DashboardComponents.TcpListeners(tcpListeners = tcpListeners) ]
    ]
