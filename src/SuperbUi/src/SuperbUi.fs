namespace SuperbUi

open Browser.Dom
open Feliz
open Feliz.Router

open SuperbUi.Dashboard
open SuperbUi.Experience

module App =
  Fable.Core.JsInterop.importSideEffects "./styles.css"

  [<ReactComponent>]
  let Router () =
    let (currentUrl, updateUrl) = React.useState (Router.currentUrl ())

    React.router [
      router.pathMode
      router.onUrlChanged updateUrl
      router.children [
        match currentUrl with
        | [] -> Dashboard.DashboardScreen()
        | notFoundPath -> Experience.NotFoundErrorScreen notFoundPath
      ]
    ]

  let root = ReactDOM.createRoot (document.getElementById "feliz-app")
  root.render (Router())
