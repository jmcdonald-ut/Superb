namespace SuperbUi.Experience

open Feliz
open Feliz.DaisyUI
open Feliz.Router

open SuperbUi.Shared.LayoutComponents

module Experience =
  let notFoundTitle = "This is uncharted territory"
  let goToDashboard = "Go to dashboard"

  [<ReactComponent>]
  let NotFoundErrorScreen (notFoundPath: string list) =
    let pathString = String.concat "/" notFoundPath
    let handleDashboardClick _ = Router.navigatePath ""

    let notFoundExplanation =
      sprintf "Seriously, we can't find a matching page for /%s" pathString

    let heroContentChildren =
      Html.div [
        prop.className "max-w-md"
        prop.children [
          Html.h1 [ prop.className "text-5xl font-bold"; prop.text notFoundTitle ]
          Html.p [ prop.className "py-6"; prop.text notFoundExplanation ]
          Daisy.button.button [ button.primary; prop.onClick handleDashboardClick; prop.text goToDashboard ]
        ]
      ]

    StandardLayout [
      Daisy.hero [
        prop.className "flex-1 h-full"
        prop.children [
          Daisy.heroContent [ prop.className "text-center"; prop.children heroContentChildren ]
        ]
      ]
    ]
