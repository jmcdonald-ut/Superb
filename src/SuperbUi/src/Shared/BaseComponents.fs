namespace SuperbUi.Shared

open Feliz
open Feliz.DaisyUI

module BaseComponents =
  [<ReactComponent>]
  let ExternalLink (href: string, text: string) =
    Daisy.link [ prop.href href; prop.target "_blank"; prop.text text ]

  [<ReactComponent>]
  let ListItem (text: string) = Html.li text

  [<ReactComponent>]
  let ErrorAlert (errors: string seq) =
    if Seq.length errors > 0 then
      Daisy.alert [
        alert.error
        prop.role "alert"
        prop.children [ Html.i [ prop.className "bx bx-error-circle" ]; Html.span (Seq.head errors) ]
      ]
    else
      Html.none
