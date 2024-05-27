namespace SuperbUi

open Feliz
open Browser.Dom

module Main =
  Fable.Core.JsInterop.importSideEffects "./styles.css"
  let root = ReactDOM.createRoot (document.getElementById "feliz-app")
  root.render (Containers.DashboardContainer())
