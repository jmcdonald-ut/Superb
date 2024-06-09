namespace SuperbUi.MySQLClient

open Fable.Core.JsInterop
open Feliz
open Feliz.DaisyUI

open SuperbUi.MySQLClient.Types
open SuperbUi.Shared.Types

module Components =
  [<ReactComponent>]
  let private SkeletonLi (_) =
    Html.li [
      prop.className "m-t-2"
      prop.children (Daisy.skeleton [ prop.key "skeleton"; prop.className "min-w-20 max-w-[75%] h-4 mt-2" ])
    ]

  let private buildSkeletonListItems itemCount = List.init itemCount SkeletonLi

  /// <summary>
  /// Form control for selecting a DB (schema) from a list of DBs.
  /// </summary>
  [<ReactComponent>]
  let SelectSchema (schemas: Schema list) (selected: SelectedSchema) (onSelect: SelectSchemaHandler) =
    let renderOption ({ schemaName = name }: Schema) =
      Html.option [ prop.value name; prop.text name ]

    let currentlySelected =
      match selected with
      | None -> ""
      | Some({ schemaName = name }) -> name

    let placeholder =
      Html.option [ prop.disabled true; prop.value ""; prop.text "Open DB" ]

    let allOptions = placeholder :: (List.map renderOption schemas)

    let select =
      Daisy.select [
        select.primary
        select.bordered
        prop.value currentlySelected
        prop.onChange (fun (ev: Browser.Types.Event) ->
          let selectedString = ev.target?value |> string

          let maybeSelected =
            Seq.tryFind (fun (schema: Schema) -> schema.schemaName = selectedString) schemas

          onSelect maybeSelected)
        prop.children allOptions
      ]

    Daisy.formControl select

  /// <summary>
  /// Presents MySQL schemata, a control for selecting the schema, and the
  /// schema's tables.
  /// </summary>
  [<ReactComponent>]
  let MySQLItems (loadingStatus) (connectedSchemaSelect: ReactElement) (tables: SchemaTable list) =
    let listItemsOfTables =
      match loadingStatus with
      | HasNotStarted
      | IsLoading -> buildSkeletonListItems 50
      | _anythingElse -> tables |> List.map _.tableName |> List.map Html.li

    Daisy.card [
      card.bordered
      prop.children [
        Daisy.cardBody [
          Daisy.cardTitle [ prop.className "font-black"; prop.text "Items" ]
          Html.div [ prop.key "select-div"; prop.children connectedSchemaSelect ]
          Html.ul listItemsOfTables
        ]
      ]
    ]
