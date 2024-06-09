namespace SuperbUi.Dashboard

open Feliz

open SuperbGraphQL
open SuperbUi.Dashboard.Types
open SuperbUi.Shared.Hooks

[<AutoOpen>]
module private InternalGraphQLHelpers =
  let client: SuperbGraphQLGraphqlClient =
    SuperbGraphQLGraphqlClient(url = "https://localhost:7011/graphql")

  let errorMessage ({ message = message }: SuperbGraphQL.ErrorType) : string = message
  let mapToErrorMessages = List.map errorMessage

  // Transforms list of option values to list of unwrapped Some values; None
  // values are dropped.
  let normalizeListOfOptions list = List.choose id list

module Hooks =
  [<Hook>]
  let useTcpListeners () =
    useGraphQLDeferred {
      initialResult = []
      mapExceptionToError = (fun (exn: exn) -> [ exn.ToString() ])
      operation =
        async {
          match! client.GetTcpListeners() with
          | Ok { tcpListeners = Some(list) } -> return list |> normalizeListOfOptions |> Ok
          | Ok { tcpListeners = None } -> return [] |> Ok
          | Error reasons -> return reasons |> mapToErrorMessages |> Error
        }
      operationDependencies = [||]
    }

  [<Hook>]
  let useHackerNewsStories () =
    useGraphQLDeferred {
      initialResult = []
      mapExceptionToError = (fun (exn: exn) -> [ exn.ToString() ])
      operation =
        async {
          match! client.GetHackerNewsStories() with
          | Ok { hackerNewsStories = Some(list) } -> return list |> normalizeListOfOptions |> Ok
          | Ok { hackerNewsStories = None } -> return [] |> Ok
          | Error reasons -> return reasons |> mapToErrorMessages |> Error
        }
      operationDependencies = [||]
    }

  [<Hook>]
  let useRedisCLI () : RedisCLIHook =
    let (command: string, setCommand) = React.useState ("")
    let (errors: string list, setErrors) = React.useState ([])
    let (executed: RedisIOEntry list, updateExecuted) = React.useStateWithUpdater ([])

    let (|Prefix|_|) (prefix: string) (subject: string) =
      if subject.StartsWith(prefix) then
        Some(subject.Substring(prefix.Length))
      else
        None

    let executeRedisCLICommand () =
      async {
        try
          let captureResult (output: string) (didFail: bool) =
            let inputOutput: RedisIOEntry = {
              input = command
              output = output
              didFail = didFail
            }

            updateExecuted (fun list -> inputOutput :: list)

          match! client.ExecuteRedisCLICommand { command = Some(command) } with
          | Ok({ executeRedisCLICommand = option }) ->
            match option with
            | Some(Prefix "ERROR: " result) -> captureResult result true
            | Some(result) -> captureResult result false
            | None -> [ "No result?" ] |> setErrors |> ignore
          | Error errors -> errors |> mapToErrorMessages |> setErrors |> ignore
        with err ->
          [ sprintf "%A" err ] |> setErrors |> ignore
      }

    let doExecuteRedisCLICommand =
      React.useCallback (executeRedisCLICommand >> Async.StartImmediate, [| box command |])

    {
      command = command
      errors = errors
      executed = executed
      executeRedisCLICommand = doExecuteRedisCLICommand
      setCommand = setCommand
    }
