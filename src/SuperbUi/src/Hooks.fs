namespace SuperbUi

open Feliz
open SuperbGraphQL

type LoadableListHook<'ValueType> = {
  clearErrors: unit -> unit
  data: 'ValueType list
  errors: ErrorType list
  load: unit -> unit
}

type RedisInputOutput = {
  didFail: bool
  input: string
  output: string
}

type RedisCLIHook = {
  command: string
  errors: ErrorType list
  executed: RedisInputOutput list
  executeRedisCLICommand: unit -> unit
  setCommand: string -> unit
}

module Hooks =
  /// <summary>
  /// Shared GraphQL client.
  /// </summary>
  let private client: SuperbGraphQLGraphqlClient =
    SuperbGraphQLGraphqlClient(url = "https://localhost:7011/graphql")

  let private error (message: string) : SuperbGraphQL.ErrorType = { message = message }

  let private normalizeListOfOptions (list: 'ValueType option list) =
    let intoNewListIfSomething (item: 'ValueType option) (newList: 'ValueType list) =
      match item with
      | Some listener -> listener :: newList
      | None -> newList

    List.foldBack intoNewListIfSomething list []

  [<Hook>]
  let useListFromServer (func: unit -> Async<Result<'ValueType list, list<ErrorType>>>) =
    let (data: 'ValueType list, setData) = React.useState ([])
    let (errors: ErrorType list, setErrors) = React.useState ([])

    let loader () =
      async {
        try
          let dataIsEmpty = [ error "Data list was empty...this might be an error?" ]

          match! func () with
          | Ok [] -> dataIsEmpty |> setErrors |> ignore
          | Ok list -> list |> setData |> ignore
          | Error reasons -> reasons |> setErrors |> ignore
        with err ->
          [ error (sprintf "%A" err) ] |> setErrors |> ignore
      }

    let clearErrors = React.useCallbackRef (fun () -> setErrors [])
    let doLoad = React.useCallbackRef (loader >> Async.StartImmediate)
    React.useEffectOnce doLoad

    {
      data = data
      errors = errors
      load = doLoad
      clearErrors = clearErrors
    }

  [<Hook>]
  let useTcpListener () =
    let loadTcpListenersAsync () =
      async {
        match! client.GetTcpListeners() with
        | Ok { tcpListeners = Some(list) } -> return list |> normalizeListOfOptions |> Ok
        | Ok { tcpListeners = None } -> return [] |> Ok
        | Error reasons -> return reasons |> Error
      }

    useListFromServer loadTcpListenersAsync

  [<Hook>]
  let useHackerNewsStories () =
    let loadHackerNewsAsync () =
      async {
        match! client.GetHackerNewsStories() with
        | Ok { hackerNewsStories = Some(list) } -> return list |> normalizeListOfOptions |> Ok
        | Ok { hackerNewsStories = None } -> return [] |> Ok
        | Error reasons -> return reasons |> Error
      }

    useListFromServer loadHackerNewsAsync

  [<Hook>]
  let useRedisCLI () : RedisCLIHook =
    let (command: string, setCommand) = React.useState ("")
    let (errors: ErrorType list, setErrors) = React.useState ([])

    let (executed: RedisInputOutput list, updateExecuted) =
      React.useStateWithUpdater ([])

    let (|Prefix|_|) (prefix: string) (subject: string) =
      if subject.StartsWith(prefix) then
        Some(subject.Substring(prefix.Length))
      else
        None

    let executeRedisCLICommand () =
      async {
        try
          let captureResult (output: string) (didFail: bool) =
            let inputOutput: RedisInputOutput = {
              input = command
              output = output
              didFail = didFail
            }

            updateExecuted (fun list -> inputOutput :: list) |> ignore

          match! client.ExecuteRedisCLICommand { command = Some(command) } with
          | Ok({ executeRedisCLICommand = option }) ->
            match option with
            | Some(Prefix "ERROR: " result) -> captureResult result true
            | Some(result) -> captureResult result false
            | None -> [ error "No result?" ] |> setErrors |> ignore
          | Error errors -> errors |> setErrors |> ignore
        with err ->
          [ error (sprintf "%A" err) ] |> setErrors |> ignore
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
