namespace SuperbUi.Shared

module Types =
  /// State of the deferred GraphQL operation.
  type GraphQLDeferredStatus =
    | HasNotStarted
    | IsLoading
    | LoadFailed
    | LoadedError
    | LoadedOk

  /// 4-arity pair:
  ///
  /// 1. Current state
  /// 2. Current result
  /// 3. Last successful result; initial result when no successful calls
  ///    have occurred.
  /// 4. Current error
  type GraphQLDeferred<'Result, 'Error> = (GraphQLDeferredStatus * 'Result * 'Result * 'Error option)

  /// Argument passed to <code>useGraphQLDeferred</code>.
  type GraphQLDeferredArg<'Result, 'Error> = {
    initialResult: 'Result
    mapExceptionToError: exn -> 'Error
    operation: Async<Result<'Result, 'Error>>
    operationDependencies: obj array
  }
