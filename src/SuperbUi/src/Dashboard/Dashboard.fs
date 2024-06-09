namespace SuperbUi.Dashboard

open Feliz
open Feliz.DaisyUI

open SuperbUi.Dashboard.Components
open SuperbUi.Shared.LayoutComponents

[<AutoOpen>]
module private Memoized =
  let MySQLModule = React.memo MySQLDashboardModule
  let HackerNewModule = React.memo HackerNewsDashboardModule
  let TcpListenersModule = React.memo TcpListenersDashboardModule
  let RedisModule = React.memo RedisDashboardModule

[<RequireQualifiedAccess>]
module Dashboard =
  /// <summary>
  /// A superb dashboard ;)
  /// </summary>
  [<ReactComponent>]
  let DashboardScreen () =
    StandardLayout [
      Html.div [
        theme.nord
        prop.className "px-6 w-full mx-auto grid grid-cols-1 lg:grid-cols-2 py-4 gap-4"
        prop.children [
          Html.div [
            prop.className "flex flex-col gap-4"
            prop.children [ TcpListenersModule(); HackerNewModule() ]
          ]
          Html.div [
            prop.className "flex flex-col gap-4"
            prop.children [ MySQLModule(); RedisModule() ]
          ]
        ]
      ]
    ]
