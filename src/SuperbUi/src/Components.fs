namespace SuperbUi

open Feliz
open Feliz.DaisyUI
open Feliz.Router

type Components() =
  [<ReactComponent>]
  static member NavBar() =
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
              prop.className "px-1"
              prop.children [
                Html.li [ Html.a [ prop.href (Router.formatPath ""); prop.text "Dashboard" ] ]
              ]
            ]
          ]
        ]
      ]
    ]

  [<ReactComponent>]
  static member StandardLayout(children: ReactElement list) =
    Html.div ((Components.NavBar()) :: children)

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
