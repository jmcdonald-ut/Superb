namespace SuperbUi.MySQLClient

open Fable.Core.JsInterop
open Feliz
open Feliz.DaisyUI

open SuperbUi.MySQLClient.Types
open SuperbUi.Shared.Types

module Components =
  // TODO: Shared component?
  [<ReactComponent>]
  let CopiedTable (children: ReactElement seq) =
    Html.div [
      prop.className "flex-1 overflow-auto font-mono"
      prop.children [
        Daisy.table [ table.sm; table.zebra; prop.className "min-w-full"; prop.children children ]
      ]
    ]

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
        select.sm
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

  [<ReactComponent>]
  let MySQLTableCell (value: string) =
    Html.td [
      prop.className "max-w-80 overflow-x-hidden text-nowrap text-ellipsis"
      prop.text value
    ]

  [<ReactComponent>]
  let MySQLTableRow (row: TableRow) =
    row.values |> (List.map _.value) |> List.map MySQLTableCell |> Html.tr

  [<ReactComponent>]
  let MySQLTableRows (rows: TableRow list) (columns: TableColumn list) =
    Daisy.card [
      card.bordered
      card.compact
      prop.className "max-h-full"
      prop.children [
        Daisy.cardBody [
          prop.className "flex flex-column overflow-hidden"
          prop.children [
            Daisy.cardTitle [ prop.className "font-black"; prop.text "Rows" ]
            CopiedTable [
              Html.thead (columns |> (List.map _.columnName) |> List.map Html.th)
              Html.tableBody (List.map MySQLTableRow rows)
            ]
          ]
        ]
      ]
    ]

  [<ReactComponent>]
  let MySQLSelectableTable
    (selectedTable: SelectedTable)
    (onSelectedTableChange: SelectedTable -> unit)
    (table: SchemaTable)
    =
    let handleSelectedTableChange _ = onSelectedTableChange (Some table)

    let className =
      match selectedTable with
      | Some({ tableName = actual }) when actual = table.tableName -> "active"
      | _else -> ""

    // TODO: Use selectedTable to determine if the button is active (and apply
    //   a style signaling that).
    Html.li [
      Html.button [
        prop.className className
        prop.onClick handleSelectedTableChange
        prop.text table.tableName
      ]
    ]

  /// <summary>
  /// Presents MySQL schemata, a control for selecting the schema, and the
  /// schema's tables.
  /// </summary>
  [<ReactComponent>]
  let MySQLItems
    (loadingStatus)
    (connectedSchemaSelect: ReactElement)
    (tables: SchemaTable list)
    (selectedTable: SelectedTable)
    (onSelectedTableChange: SelectedTable -> unit)
    =
    let listItemsOfTables =
      match loadingStatus with
      | HasNotStarted
      | IsLoading -> buildSkeletonListItems 50
      | _anythingElse -> tables |> List.map (MySQLSelectableTable selectedTable onSelectedTableChange)

    Daisy.card [
      card.bordered
      card.compact
      prop.className "max-h-full"
      prop.children [
        Daisy.cardBody [
          prop.className "flex flex-column overflow-hidden"
          prop.children [
            Daisy.cardTitle [ prop.className "font-black"; prop.text "Items" ]
            Html.div [ prop.key "select-div"; prop.children connectedSchemaSelect ]
            Daisy.menu [
              menu.sm
              prop.className "flex-1 flex-nowrap overflow-scroll gap-1 px-0"
              prop.children listItemsOfTables
            ]
          ]
        ]
      ]
    ]
