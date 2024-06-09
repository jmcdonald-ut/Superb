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
    let (_schemasLoadingStatus, schemas, _prior, _errors) = useSchemata ()
    let (selected, setSelected) = React.useState<Schema option> (None)
    let schemaSelect = SelectSchema schemas selected setSelected
    let (loadingStatus, tables, _prior, _errors) = useTables selected

    StandardLayout [
      Html.div [
        theme.nord
        prop.className "px-6 w-full mx-auto grid grid-cols-3 py-4 gap-4"
        prop.children [
          Html.div [
            prop.className "col col-span-1"
            prop.children [ MySQLItems loadingStatus schemaSelect tables ]
          ]
          Html.div [ prop.className "col col-span-2"; prop.text "<PLACEHOLDER>" ]
        ]
      ]
    ]
