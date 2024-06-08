namespace SuperbUi

open Feliz
open Feliz.DaisyUI
open Feliz.Router

type private NavItem = string
type private NavItemPair = (string * string list)

type Components() =
  [<ReactComponent>]
  static member NavBar() =
    let items: NavItemPair list = [ ("MySQL", [ "mysql" ]); ("Dashboard", []) ]

    let activePath = Router.currentPath ()

    let renderItem (item: NavItemPair) =
      match item with
      | (label, path) when path = activePath ->
        Html.li [
          Html.a [ prop.className "active"; prop.href (Router.formatPath path); prop.text label ]
        ]
      | (label, path) -> Html.li [ Html.a [ prop.href (Router.formatPath path); prop.text label ] ]

    Daisy.navbar [
      prop.className "bg-base-100"
      prop.children [
        Html.div [
          prop.className "flex-1"
          prop.children [
            Html.a [
              prop.className "btn btn-ghost text-xl text-2xl font-black"
              prop.href (Router.formatPath "")
              prop.text "Superb"
            ]
          ]
        ]
        Html.div [
          prop.className "flex-none"
          prop.children [
            Daisy.menu [
              menu.horizontal
              prop.className "px-1 gap-1"
              prop.children (Seq.map renderItem items)
            ]
          ]
        ]
      ]
    ]

  [<ReactComponent>]
  static member StandardLayout(children: ReactElement list) =
    Html.div [
      prop.className "flex flex-col h-screen"
      prop.children [
        Components.NavBar()
        Html.div [ prop.className "flex-1 overflow-y-scroll"; prop.children children ]
      ]
    ]

  [<ReactComponent>]
  static member ExternalLink(href: string, text: string) =
    Daisy.link [ prop.href href; prop.target "_blank"; prop.text text ]

  [<ReactComponent>]
  static member ListItem(text: string) = Html.li text

  [<ReactComponent>]
  static member ErrorAlert(errors: SuperbGraphQL.ErrorType seq) =
    if Seq.length errors > 0 then
      Daisy.alert [
        alert.error
        prop.role "alert"
        prop.children [
          Html.i [ prop.className "bx bx-error-circle" ]
          Html.span (Seq.head errors).message
        ]
      ]
    else
      Html.none
