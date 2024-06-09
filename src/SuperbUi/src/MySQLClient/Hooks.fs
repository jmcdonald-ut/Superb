namespace SuperbUi.MySQLClient

open Feliz

open SuperbGraphQL
open SuperbUi.Shared.Hooks
open SuperbUi.Shared.Types
open SuperbUi.MySQLClient.Types

// TODO: Swap out other `SuperbGraphQL` types here with the simpler
//   `MySQLClient.Types`.
[<AutoOpen>]
module private InternalHelpers =
  let client: SuperbGraphQLGraphqlClient =
    SuperbGraphQLGraphqlClient(url = "https://localhost:7011/graphql")

  // Transforms list of option values to list of unwrapped Some values; None
  // values are dropped.
  let normalizeListOfOptions list = List.choose id list

module Hooks =
  [<Hook>]
  let useSchemata () =
    useGraphQLDeferred {
      initialResult = []
      mapExceptionToError = (fun exn -> [ exn.ToString() ])
      operation =
        async {
          match! client.GetSchemata() with
          | Ok { schemata = Some(list) } -> return list |> normalizeListOfOptions |> Ok
          | Ok { schemata = None } -> return [] |> Ok
          | Error reasons -> return reasons |> List.map (fun (e: ErrorType) -> e.message) |> Error
        }
      operationDependencies = [||]
    }

  [<Hook>]
  let useTables (schema: GetSchemata.SchemaType option) =
    useGraphQLDeferred {
      initialResult = []
      mapExceptionToError = (fun exn -> [ exn.ToString() ])
      operation =
        async {
          match schema with
          | None -> return Ok []
          | Some({ schemaName = name }) ->
            match! client.GetTables({ schemaName = name }) with
            | Ok { tables = Some(list) } -> return list |> normalizeListOfOptions |> Ok
            | Ok { tables = None } -> return [] |> Ok
            | Error reasons -> return reasons |> List.map (fun (e: ErrorType) -> e.message) |> Error
        }
      operationDependencies = [| box schema |]
    }

  // TODO: Refactor this hook so it doesn't depend on `selectedSchema`. We only
  //   need the schema name, and that's available on the table record.
  [<Hook>]
  let useGetSampleOfTableRows (count: int) (selectedSchema: SelectedSchema) (selectedTable: SelectedTable) =
    useGraphQLDeferred {
      initialResult = []
      mapExceptionToError = (fun exn -> [ exn.ToString() ])
      operation =
        async {
          let msg = sprintf "%A %A" selectedSchema selectedTable
          Browser.Dom.console.log msg
          Browser.Dom.console.log "Hello"

          match (selectedSchema, selectedTable) with
          | (None, _)
          | (_, None) -> return Ok []
          | (Some({ schemaName = schemaName }), Some({ tableName = tableName })) ->
            let input: GetSampleOfTableRows.InputVariables = {
              count = count
              schemaName = schemaName
              tableName = tableName
            }

            match! client.GetSampleOfTableRows(input) with
            | Ok { tableRows = Some(rows) } -> return rows |> normalizeListOfOptions |> Ok
            | Ok { tableRows = None } -> return Ok []
            | Error reasons -> return reasons |> (List.map _.message) |> Error
        }
      operationDependencies = [| box count; box selectedSchema; box selectedTable |]
    }
