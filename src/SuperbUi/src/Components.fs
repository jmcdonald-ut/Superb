namespace SuperbUi

open Feliz
open Feliz.DaisyUI

type Components() =
  [<ReactComponent>]
  static member ExternalLink(href: string, text: string) =
    Daisy.link [ prop.href href; prop.target "_blank"; prop.text text ]

  [<ReactComponent>]
  static member ListItem(text: string) = Html.li text
