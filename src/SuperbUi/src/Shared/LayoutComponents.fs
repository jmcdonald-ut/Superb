namespace SuperbUi.Shared

open Feliz
open Feliz.DaisyUI
open Feliz.Router

type private NavLabel = string
type private NavItem = string
type private NavItemPair = (NavLabel * NavItem list)

module LayoutComponents =
  [<ReactComponent>]
  let NavBar () =
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
  let StandardLayout (children: ReactElement list) =
    Html.div [
      prop.className "flex flex-col h-screen"
      prop.children [
        NavBar()
        Html.div [ prop.className "flex-1 overflow-y-scroll"; prop.children children ]
      ]
    ]
