namespace SuperbUi

open Feliz
open Feliz.DaisyUI

type Memoized =
  static member TopHackerNewsStories = React.memo (DashboardComponents.TopHackerNewsStories)
  static member TcpListeners = React.memo (DashboardComponents.TcpListeners)
  static member RedisCLI = React.memo (DashboardComponents.RedisCLI)

type Containers =
  /// <summary>
  /// A superb dashboard ;)
  /// </summary>
  [<ReactComponent>]
  static member DashboardContainer() =
    Html.div [
      theme.nord
      prop.className "container mx-auto grid grid-cols-1 lg:grid-cols-2 py-8 gap-4"
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
