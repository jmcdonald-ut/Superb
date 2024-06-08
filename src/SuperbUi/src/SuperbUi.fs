namespace SuperbUi

open Feliz
open Feliz.Router
open Browser.Dom
open SuperbUi.Dashboard

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
        | [] -> Dashboard.DashboardContainer()
        | _otherwise -> ErrorScreen.Container "That page doesn't seem to be found!"
      ]
    ]

  let root = ReactDOM.createRoot (document.getElementById "feliz-app")
  root.render (Router())
