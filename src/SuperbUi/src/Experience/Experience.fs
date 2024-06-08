namespace SuperbUi.Experience

open Feliz
open Feliz.DaisyUI
open Feliz.Router

open SuperbUi

module Experience =
  [<ReactComponent>]
  let NotFoundErrorScreen (notFoundPath: string list) =
    let notFoundExplanation =
      notFoundPath
      |> String.concat "/"
      |> sprintf "Seriously, we can't find a matching page for /%s"

    Html.div [
      prop.className "flex flex-col min-h-screen"
      prop.children [
        Components.NavBar()
        Daisy.hero [
          prop.className "flex-1"
          prop.children [
            Daisy.heroContent [
              prop.className "text-center"
              prop.children [
                Html.div [
                  prop.className "max-w-md"
                  prop.children [
                    Html.h1 [ prop.className "text-5xl font-bold"; prop.text "This is uncharted territory" ]
                    Html.p [ prop.className "py-6"; prop.text notFoundExplanation ]
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
      ]
    ]
