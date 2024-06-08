namespace SuperbUi.Dashboard.Components

open Feliz
open Feliz.DaisyUI

open SuperbUi

[<RequireQualifiedAccessAttribute>]
module DashboardModule =
  type Props = {
    children: ReactElement seq
    moduleName: string
    title: string
    errors: SuperbGraphQL.ErrorType list
  }

  /// <summary>
  /// Renders a standalone dashboard module.
  /// </summary>
  [<ReactComponent>]
  let ModuleWidget (moduleProps: Props) =
    let renderedErrors = Components.ErrorAlert(moduleProps.errors)

    Daisy.card [
      card.bordered
      prop.children [
        Daisy.cardBody [
          Html.div [
            prop.className "flex place-content-between align-items-end"
            prop.children [ Daisy.cardTitle [ prop.className "font-black"; prop.text moduleProps.title ] ]
          ]
          Html.div [
            prop.className "flex flex-col gap-2"
            prop.children (Seq.insertAt 0 renderedErrors moduleProps.children)
          ]
        ]
      ]
    ]
