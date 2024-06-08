namespace SuperbUi

open Browser.Dom
open Feliz
open Feliz.Router

open SuperbUi.Dashboard
open SuperbUi.Experience
open SuperbUi.MySQLClient

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
        | [ "mysql" ] -> MySQLClient.SchemataScreen()
        | notFoundPath -> Experience.NotFoundErrorScreen notFoundPath
      ]
    ]

  let root = ReactDOM.createRoot (document.getElementById "feliz-app")
  root.render (Router())
