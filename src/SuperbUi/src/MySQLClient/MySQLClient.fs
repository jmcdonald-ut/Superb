namespace SuperbUi.MySQLClient

open Feliz
open Feliz.DaisyUI

open SuperbUi.MySQLClient.Components
open SuperbUi.MySQLClient.Hooks
open SuperbUi.MySQLClient.Types
open SuperbUi.Shared.LayoutComponents

// TODO: Handle overflowing content better so individual cards are scrollable.
//   This would be preferable to the entire page being scrollable.
module MySQLClient =
  [<ReactComponent>]
  let TablesContainer (selectedSchema: SelectedSchema) (selectedTable: SelectedTable) =
    let (_rowsLoadingStatus, rows, _prior, _errors) =
      useGetSampleOfTableRows 50 selectedSchema selectedTable

    MySQLTableRows rows

  [<ReactComponent>]
  let SchemataScreen () =
    let (_schemasLoadingStatus, schemas, _prior, _errors) = useSchemata ()
    let (selectedSchema, setSelectedSchema) = React.useState<SelectedSchema> (None)
    let (selectedTable, setSelectedTable) = React.useState<SelectedTable> (None)
    let schemaSelect = SelectSchema schemas selectedSchema setSelectedSchema
    let (loadingStatus, tables, _prior, _errors) = useTables selectedSchema

    StandardLayout [
      Html.div [
        theme.nord
        prop.className "px-6 w-full mx-auto grid grid-cols-3 py-4 gap-4"
        prop.children [
          Html.div [
            prop.className "col col-span-1"
            prop.children [ MySQLItems loadingStatus schemaSelect tables selectedTable setSelectedTable ]
          ]
          Html.div [
            prop.className "col col-span-2"
            prop.children [ TablesContainer selectedSchema selectedTable ]
          ]
        ]
      ]
    ]
