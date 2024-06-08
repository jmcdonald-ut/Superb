namespace SuperbUi.MySQLClient

open SuperbGraphQL

module Types =
  type Schema = GetSchemata.SchemaType
  type SchemaTable = GetTables.TableType
  type SelectedSchema = Schema option
  type SelectSchemaHandler = SelectedSchema -> unit

  type MapExceptionToErrorFun<'Error> = exn -> 'Error

  type DeferredMySQLState =
    | HasNotStarted
    | IsLoading
    | LoadFailed
    | LoadedError
    | LoadedOk

  /// 4-arity pair with: hook state, current content, prior (successfully)
  /// loaded content, and current error.
  ///
  /// TODO: Refactor this to something like DeferredGraphQLContent.
  type DeferredMySQLContent<'Content, 'Error> = (DeferredMySQLState * 'Content * 'Content * 'Error option)

  type DeferredMySQLContentArg<'Content, 'Error> = {
    initialContent: 'Content
    mapExceptionToError: exn -> 'Error
    operation: Async<Result<'Content, 'Error>>
    operationDependencies: obj array
  }
