namespace SuperbUi.Dashboard.Components

open Feliz
open Feliz.DaisyUI
open SuperbUi

[<RequireQualifiedAccess>]
module MySQLExplorer =
  type SchemaRowProps = {
    onClick: string -> unit
    schema: SuperbGraphQL.GetSchemata.SchemaType
  }

  /// <summary>
  /// Presents top Hacker News stories.
  /// </summary>
  [<ReactComponent>]
  let SchemaRow ({ onClick = onClick; schema = schema }: SchemaRowProps) =
    Html.tr [
      prop.className "align-top"
      prop.children [
        Html.td [ prop.className "align-top"; prop.text schema.schemaName ]
        Html.td [ prop.className "align-top"; prop.text schema.catalogName ]
        Html.td [ prop.className "align-top"; prop.text schema.defaultCharacterSetName ]
        Html.td [ prop.className "align-top"; prop.text schema.defaultCollationName ]
        Html.td [ prop.className "align-top"; prop.text schema.defaultEncryption ]
      ]
      prop.onClick (fun _ -> onClick schema.schemaName)
    ]

  /// <summary>
  /// Presents top Hacker News stories.
  /// </summary>
  [<ReactComponent>]
  let DashboardModule () =
    let schemataState = Hooks.useSchemata ()
    let (selectedSchema, setSchema) = React.useState ("")

    let renderSchemaRow =
      React.useCallback ((fun schema -> SchemaRow { onClick = setSchema; schema = schema }), [| setSchema |])

    React.useEffect (
      (fun () ->
        Browser.Dom.console.log ((sprintf "Updated %s" selectedSchema)) |> ignore
        ()),
      [| box selectedSchema |]
    )

    DashboardModule.ModuleWidget {
      moduleName = "mysql-explorer"
      title = "MySQL"
      children = [
        Html.div [
          prop.className "overflow-x-auto"
          prop.children [
            Daisy.table [
              prop.className "table-auto overflow-scroll w-full table-sm"
              prop.children [
                Html.thead [
                  Html.tr [
                    Html.th "Schema"
                    Html.th "Catalog"
                    Html.th "Default Character Set"
                    Html.th "Default Collation"
                    Html.th "Default Encryption"
                  ]
                ]
                Html.tbody (Seq.map renderSchemaRow schemataState.data)
              ]
            ]
          ]
        ]
      ]
      errors = schemataState.errors
    }
