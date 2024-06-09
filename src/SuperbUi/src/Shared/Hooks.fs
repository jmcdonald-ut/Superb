namespace SuperbUi.Shared

open Feliz
open Feliz.UseDeferred

open SuperbUi.Shared.Types

[<AutoOpen>]
module private UseGraphQLDeferredHelpers =
  let mapToHasNotStarted (arg: GraphQLDeferredArg<'Content, 'Error>) (_: GraphQLDeferred<'Content, 'Error>) =
    (HasNotStarted, arg.initialResult, arg.initialResult, None)

  let mapToLoadedError (reason: 'Error) ((_, current, prior, _): GraphQLDeferred<'Content, 'Error>) =
    (LoadedError, current, prior, Some reason)

  let mapToLoadedOk (newContent: 'Content) ((_, current, _, _): GraphQLDeferred<'Content, 'Error>) =
    (LoadedOk, newContent, current, None)

  let mapToLoadFailed (reason: 'Error) ((_, current, prior, _): GraphQLDeferred<'Content, 'Error>) =
    (LoadFailed, current, prior, Some reason)

  let mapToIsLoading (emptyContent: 'Content) ((_, _, prior, _): GraphQLDeferred<'Content, 'Error>) =
    (IsLoading, emptyContent, prior, None)

module Hooks =
  [<Hook>]
  let useGraphQLDeferred<'Content, 'Error> (arg: GraphQLDeferredArg<'Content, 'Error>) =
    let initialPair = (HasNotStarted, arg.initialResult, arg.initialResult, None)
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
        | Deferred.InProgress -> updatePair (mapToIsLoading arg.initialResult)),
      [| box deferred |]
    )

    pair
