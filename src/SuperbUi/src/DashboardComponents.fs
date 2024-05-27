namespace SuperbUi

open Feliz

type DashboardComponents() =
  /// <summary>
  /// Renders a standalone dashboard module.
  /// </summary>
  [<ReactComponent>]
  static member DashboardModule(moduleName: string, title: string, children: ReactElement seq) =
    Html.section [
      prop.className moduleName
      prop.children [ Html.h3 title; Html.div children ]
    ]

  /// <summary>
  /// Presents a single TCP listener as a table row.
  /// </summary>
  [<ReactComponent>]
  static member TcpRow(tcpListener: GraphQLClient.TcpListener) =
    let toLi (host: string) = Html.li host

    Html.tr [
      Html.td tcpListener.processId
      Html.td tcpListener.user
      Html.td tcpListener.command
      Html.td [ Html.ul (Seq.map toLi tcpListener.hosts) ]
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
        Html.table [
          Html.thead [
            Html.tr [ Html.th "Process"; Html.th "User"; Html.th "Command"; Html.th "Hosts" ]
          ]
          Html.tbody (Seq.map toTcpRow tcpListeners)
        ]
      ]
    )
