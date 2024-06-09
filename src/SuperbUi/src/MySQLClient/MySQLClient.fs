namespace SuperbUi.MySQLClient

open Feliz
open Feliz.DaisyUI

open SuperbUi.MySQLClient.Components
open SuperbUi.MySQLClient.Hooks
open SuperbUi.MySQLClient.Types
open SuperbUi.Shared.LayoutComponents

module MySQLClient =
  [<ReactComponent>]
  let SchemataScreen () =
    let (_, schemas, _, _) = useSchemata ()
    let (selected, setSelected) = React.useState<Schema option> (None)
    let schemaSelect = SelectSchema schemas selected setSelected

    let (_state, tables, _prior, _errors) = useTables selected

    React.useEffect (
      (fun () ->
        let msg = sprintf "Tables: %A" tables
        Browser.Dom.console.log msg
        ()),
      [| box tables; box selected |]
    )

    StandardLayout [
      Html.div [
        theme.nord
        prop.className "px-6 w-full mx-auto grid grid-cols-3 py-4 gap-4"
        prop.children [
          Html.div [
            prop.className "col col-span-1"
            prop.children [ MySQLItems schemaSelect tables ]
          ]
          Html.div [ prop.className "col col-span-2"; prop.text "<PLACEHOLDER>" ]
        ]
      ]
    ]
