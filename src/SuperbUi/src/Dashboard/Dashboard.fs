namespace SuperbUi.Dashboard

open Feliz
open Feliz.DaisyUI
open SuperbUi.Dashboard.Components
open SuperbUi

type private Memoized =
  // static member Schemata = React.memo (DashboardComponents.Schemata)
  static member TopHackerNewsStories = React.memo HackerNewsFeed.DashboardModule
  static member TcpListeners = React.memo TcpListeners.DashboardModule
  static member RedisCLI = React.memo RedisCLI.DashboardModule

[<RequireQualifiedAccess>]
module Dashboard =
  /// <summary>
  /// A superb dashboard ;)
  /// </summary>
  [<ReactComponent>]
  let DashboardContainer () =
    Components.StandardLayout [
      Html.div [
        theme.nord
        prop.className "px-6 w-full mx-auto grid grid-cols-1 lg:grid-cols-2 py-4 gap-4"
        prop.children [
          Html.div [
            prop.className "flex flex-col gap-4"
            prop.children [ Memoized.TcpListeners(); Memoized.RedisCLI() ]
          ]
          Html.div [
            prop.className "flex flex-col gap-4"
            prop.children [ Memoized.TopHackerNewsStories() ]
          ]
        ]
      ]
    ]
