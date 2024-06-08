namespace SuperbUi.MySQLClient

open Fable.Core.JsInterop
open Feliz
open Feliz.DaisyUI

open SuperbUi.MySQLClient.Types

module Components =
  /// <summary>
  /// Form control for selecting a DB (schema) from a list of DBs.
  /// </summary>
  [<ReactComponent>]
  let SelectSchema (schemas: Schema list) (selected: SelectedSchema) (onSelect: SelectSchemaHandler) =
    let renderOption (schema: Schema) =
      Html.option [ prop.value schema.schemaName; prop.text schema.schemaName ]

    let renderOptions = List.map renderOption

    let currentlySelected =
      match selected with
      | None -> ""
      | Some({ schemaName = name }) -> name

    let placeholder =
      Html.option [ prop.disabled true; prop.value ""; prop.text "Open DB" ]

    Daisy.formControl [
      prop.key "select-db"
      prop.children [
        Daisy.select [
          select.primary
          select.bordered
          prop.value currentlySelected
          prop.onChange (fun (ev: Browser.Types.Event) ->
            let selectedString = ev.target?value |> string

            let maybeSelected =
              Seq.tryFind (fun (schema: Schema) -> schema.schemaName = selectedString) schemas

            onSelect maybeSelected)
          prop.children (placeholder :: (renderOptions schemas))
        ]
      ]
    ]

  /// <summary>
  /// Presents MySQL schemata, a control for selecting the schema, and the
  /// schema's tables.
  /// </summary>
  [<ReactComponent>]
  let MySQLItems (connectedSchemaSelect: ReactElement) (tables: SchemaTable list) =
    let renderLi (table: SchemaTable) = Html.li table.tableName
    let renderTables = Seq.map renderLi

    Daisy.card [
      card.bordered
      prop.className "!border-base-300"
      prop.children [
        Daisy.cardBody [
          Daisy.cardTitle [ prop.className "font-black"; prop.text "Items" ]
          Html.div connectedSchemaSelect
          Html.ul [ prop.children (renderTables tables) ]
        ]
      ]
    ]
