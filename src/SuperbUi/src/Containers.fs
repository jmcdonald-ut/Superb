namespace SuperbUi

open Feliz
open Feliz.DaisyUI

type Containers =
  /// <summary>
  /// A superb dashboard ;)
  /// </summary>
  [<ReactComponent>]
  static member DashboardContainer() =
    let (tcpListeners: GraphQLClient.TcpListener list, setTcpListeners) =
      React.useState<GraphQLClient.TcpListener list> ([])

    let (hackerNewsStories: GraphQLClient.HackerNewsStory list, setHackerNewsStories) =
      React.useState<GraphQLClient.HackerNewsStory list> ([])

    let getTcpListeners () =
      async {
        match! GraphQLClient.getTcpListeners () with
        | Ok(tcpListeners: GraphQLClient.TcpListener list) -> tcpListeners |> setTcpListeners |> ignore
        | Error _ -> ()
      }

    let getHackerNewsStories () =
      async {
        match! GraphQLClient.getHackerNewsStories () with
        | Ok(stories: GraphQLClient.HackerNewsStory list) -> stories |> setHackerNewsStories |> ignore
        | Error _ -> ()
      }

    React.useEffect (getTcpListeners >> Async.StartImmediate, [||])
    React.useEffect (getHackerNewsStories >> Async.StartImmediate, [||])

    Html.div [
      theme.nord
      prop.className "container mx-auto grid grid-cols-1 lg:grid-cols-2 py-8 gap-4"
      prop.children [
        Html.div [ DashboardComponents.TcpListeners(tcpListeners = tcpListeners) ]
        Html.div [
          DashboardComponents.TopHackerNewsStories(hackerNewsStories = hackerNewsStories)
        ]
      ]
    ]
