namespace SuperbUi.Shared

open Feliz
open Feliz.DaisyUI
open Feliz.Router

type private RouteLabel = string
type private RoutePath = string list
type private RouteLabelAndPath = (RouteLabel * RoutePath)

module LayoutComponents =
  let routes: RouteLabelAndPath list = [ ("MySQL", [ "mysql" ]); ("Dashboard", []) ]

  let navigateTo (path: RoutePath) (_clickEvent) =
    path |> Router.formatPath |> Router.navigatePath

  [<ReactComponent>]
  let NavBar () =
    let currentPath = Router.currentPath ()

    let renderItem ((label, path): RouteLabelAndPath) =
      let className = if path = currentPath then "active" else ""

      Html.li [
        Html.button [ prop.className className; prop.onClick (navigateTo path); prop.text label ]
      ]

    Daisy.navbar [
      prop.className "bg-base-100"
      prop.children [
        Html.div [
          prop.className "flex-1"
          prop.children [
            Daisy.button.button [
              button.ghost
              prop.className "text-xl text-2xl font-black"
              prop.onClick (navigateTo [])
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
              prop.children (Seq.map renderItem routes)
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
