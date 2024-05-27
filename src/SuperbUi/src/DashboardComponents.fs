namespace SuperbUi

open Feliz
open Feliz.DaisyUI

type DashboardComponents() =
  /// <summary>
  /// Renders a standalone dashboard module.
  /// </summary>
  [<ReactComponent>]
  static member DashboardModule(moduleName: string, title: string, children: ReactElement seq) =
    Daisy.card [
      card.bordered
      prop.children [ Daisy.cardBody [ Daisy.cardTitle title; Html.div children ] ]
    ]

  /// <summary>
  /// Presents a single TCP listener as a table row.
  /// </summary>
  [<ReactComponent>]
  static member TcpRow(tcpListener: GraphQLClient.TcpListener) =
    let toLi (host: string) = Html.li host

    // Yeah
    Html.tr [
      prop.className "align-top"
      prop.children [
        Html.td [ prop.className "align-top"; prop.text tcpListener.processId ]
        Html.td [ prop.className "align-top"; prop.text tcpListener.user ]
        Html.td [ prop.className "align-top"; prop.text tcpListener.command ]
        Html.td [ prop.className "align-top"; prop.children [ Html.ul (Seq.map toLi tcpListener.hosts) ] ]
      ]
    ]

  /// <summary>
  /// Presents visible TCP listeners in a table.
  /// </summary>
  [<ReactComponent>]
  static member TcpListeners(tcpListeners: GraphQLClient.TcpListener seq) =
    let toTcpRow (tcpListener: GraphQLClient.TcpListener) =
      DashboardComponents.TcpRow(tcpListener = tcpListener)

    DashboardComponents.DashboardModule(
      moduleName = "tcp-listeners",
      title = "TCP Listeners",
      children = [
        Daisy.table [
          Html.thead [
            Html.tr [ Html.th "Process"; Html.th "User"; Html.th "Command"; Html.th "Hosts" ]
          ]
          Html.tbody (Seq.map toTcpRow tcpListeners)
        ]
      ]
    )
