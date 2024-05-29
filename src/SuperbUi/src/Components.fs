namespace SuperbUi

open Feliz
open Feliz.DaisyUI

type Components() =
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
