namespace SuperbUi.MySQLClient

open Feliz
open Feliz.UseDeferred

open SuperbGraphQL
open SuperbUi.MySQLClient.Types

/// TODO: Refactor <code>useDeferredMySQLContent</code> into shared namespace,
/// and generalize for GraphQL use.
module Use =
  let mapToHasNotStarted (arg: DeferredMySQLContentArg<'Content, 'Error>) (_: DeferredMySQLContent<'Content, 'Error>) =
    (HasNotStarted, arg.initialContent, arg.initialContent, None)

  let mapToLoadedError (reason: 'Error) ((_, current, prior, _): DeferredMySQLContent<'Content, 'Error>) =
    (LoadedError, current, prior, Some reason)

  let mapToLoadedOk (newContent: 'Content) ((_, current, _, _): DeferredMySQLContent<'Content, 'Error>) =
    (LoadedOk, newContent, current, None)

  let mapToLoadFailed (reason: 'Error) (pair: (DeferredMySQLState * 'Content * 'Content * 'Error option)) =
    let (_, content, priorContent, _) = pair
    (LoadFailed, content, priorContent, Some reason)

  let mapToIsLoading (emptyContent: 'Content) (pair: DeferredMySQLContent<'Content, 'Error>) =
    let (_, _, priorContent, _) = pair
    (IsLoading, emptyContent, priorContent, None)

  let useDeferredMySQLContent<'Content, 'Error> (arg: DeferredMySQLContentArg<'Content, 'Error>) =
    let initialPair = (HasNotStarted, arg.initialContent, arg.initialContent, None)
    let (pair, updatePair) = React.useStateWithUpdater (initialPair)
    let deferred = React.useDeferred (arg.operation, arg.operationDependencies)

    React.useEffect (
      (fun () ->
        match deferred with
        | Deferred.Resolved loadResult ->
          match loadResult with
          | Error reason -> updatePair (mapToLoadedError reason)
          | Ok content -> updatePair (mapToLoadedOk content)
        | Deferred.Failed exn -> updatePair (mapToLoadFailed (arg.mapExceptionToError exn))
        | Deferred.HasNotStartedYet -> updatePair (mapToHasNotStarted arg)
        | Deferred.InProgress -> updatePair (mapToIsLoading arg.initialContent)),
      [| box deferred |]
    )

    pair

module Hooks =
  let private client: SuperbGraphQLGraphqlClient =
    SuperbGraphQLGraphqlClient(url = "https://localhost:7011/graphql")

  let private normalizeListOfOptions (list: 'ValueType option list) =
    let intoNewListIfSomething (item: 'ValueType option) (newList: 'ValueType list) =
      match item with
      | Some listener -> listener :: newList
      | None -> newList

    List.foldBack intoNewListIfSomething list []

  [<Hook>]
  let useSchemata () =
    let loadSchemataAsync =
      async {
        match! client.GetSchemata() with
        | Ok { schemata = Some(list) } -> return list |> normalizeListOfOptions |> Ok
        | Ok { schemata = None } -> return [] |> Ok
        | Error reasons -> return reasons |> List.map (fun (e: ErrorType) -> e.message) |> Error
      }

    let arg = {
      initialContent = []
      mapExceptionToError = (fun exn -> [ exn.ToString() ])
      operation = loadSchemataAsync
      operationDependencies = [||]
    }

    Use.useDeferredMySQLContent arg

  [<Hook>]
  let useTables (schema: GetSchemata.SchemaType option) =
    let loadTablesAsync: Async<Result<SchemaTable list, string list>> =
      async {
        match schema with
        | None -> return Ok []
        | Some({ schemaName = name }) ->
          match! client.GetTables({ schemaName = name }) with
          | Ok { tables = Some(list) } -> return list |> normalizeListOfOptions |> Ok
          | Ok { tables = None } -> return [] |> Ok
          | Error reasons -> return reasons |> List.map (fun (e: ErrorType) -> e.message) |> Error
      }

    let arg = {
      initialContent = []
      mapExceptionToError = (fun exn -> [ exn.ToString() ])
      operation = loadTablesAsync
      operationDependencies = [| box schema |]
    }

    Use.useDeferredMySQLContent arg
