namespace SuperbUi.Dashboard.Components

open Feliz
open Feliz.DaisyUI
open SuperbUi

[<RequireQualifiedAccess>]
module TcpListeners =
  /// <summary>
  /// Presents a single TCP listener as a table row.
  /// </summary>
  [<ReactComponent>]
  let TcpRow (tcpListener: SuperbGraphQL.GetTcpListeners.TcpListenerType) =
    Html.tr [
      prop.className "align-top"
      prop.children [
        Html.td [ prop.className "align-top"; prop.text tcpListener.processId ]
        Html.td [ prop.className "align-top"; prop.text tcpListener.user ]
        Html.td [ prop.className "align-top"; prop.text tcpListener.command ]
        Html.td [
          prop.className "align-top"
          prop.children [ Html.ul (Seq.map Components.ListItem tcpListener.hosts) ]
        ]
      ]
    ]

  /// <summary>
  /// Presents visible TCP listeners in a table.
  /// </summary>
  [<ReactComponent>]
  let DashboardModule () =
    let tcpListenersState = Hooks.useTcpListener ()

    DashboardModule.ModuleWidget {
      moduleName = "tcp-listeners"
      title = "TCP Listeners"
      children = [
        Daisy.table [
          prop.className "table-sm"
          prop.children [
            Html.thead [
              Html.tr [ Html.th "Process"; Html.th "User"; Html.th "Command"; Html.th "Hosts" ]
            ]
            Html.tbody (Seq.map TcpRow tcpListenersState.data)
          ]
        ]
      ]
      errors = tcpListenersState.errors
    }
