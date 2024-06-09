namespace SuperbUi.MySQLClient

open Feliz

open SuperbGraphQL
open SuperbUi.Shared.Hooks
open SuperbUi.Shared.Types

[<AutoOpen>]
module private InternalHelpers =
  let client: SuperbGraphQLGraphqlClient =
    SuperbGraphQLGraphqlClient(url = "https://localhost:7011/graphql")

  let normalizeListOfOptions (list: 'ValueType option list) =
    let intoNewListIfSomething (item: 'ValueType option) (newList: 'ValueType list) =
      match item with
      | Some listener -> listener :: newList
      | None -> newList

    List.foldBack intoNewListIfSomething list []

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
