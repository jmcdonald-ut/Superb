namespace SuperbUi

open Feliz
open Browser.Dom

module Main =
  let root = ReactDOM.createRoot (document.getElementById "feliz-app")
  root.render (Containers.DashboardContainer())
