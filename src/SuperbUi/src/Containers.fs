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

    let (hackerNewsStories, setHackerNewsStories) =
      React.useState<GraphQLClient.HackerNewsStory list> ([])

    let getTcpListeners () =
      async {
        match! GraphQLClient.getTcpListeners () with
        | Ok tcpListeners -> tcpListeners |> setTcpListeners |> ignore
        | Error _ -> ()
      }

    let getHackerNewsStories () =
      async {
        match! GraphQLClient.getHackerNewsStories () with
        | Ok stories -> stories |> setHackerNewsStories |> ignore
        | Error _ -> ()
      }

    React.useEffect (getTcpListeners >> Async.StartImmediate, [||])
    React.useEffect (getHackerNewsStories >> Async.StartImmediate, [||])

    Html.div [
      theme.nord
      prop.className "container mx-auto grid grid-cols-2 py-4 gap-4"
      prop.children [
        Html.div [ DashboardComponents.TcpListeners(tcpListeners = tcpListeners) ]
        Html.div [
          DashboardComponents.TopHackerNewsStories(hackerNewsStories = hackerNewsStories)
        ]
      ]
    ]
