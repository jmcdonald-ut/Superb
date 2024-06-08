namespace SuperbUi

open Feliz
open Feliz.Router
open Feliz.DaisyUI

[<RequireQualifiedAccess>]
module ErrorScreen =
  [<ReactComponent>]
  let Container (reason: string) =
    Daisy.hero [
      prop.className "min-h-screen"
      prop.children [
        Daisy.heroContent [
          prop.className "text-center"
          prop.children [
            Html.div [
              prop.className "max-w-md"
              prop.children [
                Html.h1 [ prop.className "text-5xl font-bold"; prop.text "This isn't good..." ]
                Html.p [ prop.className "py-6"; prop.text reason ]
                Daisy.button.button [
                  button.primary
                  prop.onClick (fun _ -> Router.navigatePath "")
                  prop.text "Go to Dashboard"
                ]
              ]
            ]
          ]
        ]
      ]
    ]
