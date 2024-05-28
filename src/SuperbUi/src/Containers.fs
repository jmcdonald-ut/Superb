namespace SuperbUi

open Feliz
open Feliz.DaisyUI

type Containers =
  /// <summary>
  /// A superb dashboard ;)
  /// </summary>
  [<ReactComponent>]
  static member DashboardContainer() =
    let (tcpListeners: GraphQLClient.TcpListener list, setTcpListeners) =
      React.useState<GraphQLClient.TcpListener list> ([])

    let (hackerNewsStories: GraphQLClient.HackerNewsStory list, setHackerNewsStories) =
      React.useState<GraphQLClient.HackerNewsStory list> ([])

    let (redisCommands, updateResults) =
      React.useStateWithUpdater<RedisInputOutput list> ([])

    let getTcpListeners () =
      async {
        match! GraphQLClient.getTcpListeners () with
        | Ok(tcpListeners: GraphQLClient.TcpListener list) -> tcpListeners |> setTcpListeners |> ignore
        | Error _ -> ()
      }

    let getHackerNewsStories () =
      async {
        match! GraphQLClient.getHackerNewsStories () with
        | Ok(stories: GraphQLClient.HackerNewsStory list) -> stories |> setHackerNewsStories |> ignore
        | Error _ -> ()
      }

    let (|Prefix|_|) (prefix: string) (subject: string) =
      if subject.StartsWith(prefix) then
        Some(subject.Substring(prefix.Length))
      else
        None

    let executeRedisCLICommand (input: string) =
      async {
        let captureResult (output: string) (didFail: bool) =
          let inputOutput: RedisInputOutput = {
            input = input
            output = output
            didFail = didFail
          }

          Browser.Dom.console.log (sprintf "%A" inputOutput)

          updateResults (fun list -> inputOutput :: list) |> ignore

        match! GraphQLClient.executeRedisCLICommand input with
        | Ok(Prefix "ERROR: " result) -> captureResult result true
        | Ok(result) -> captureResult result false
        | Error err -> captureResult err true
      }

    let doExecuteRedisCLICommand (input: string) =
      input |> executeRedisCLICommand |> Async.StartImmediate |> ignore

    let handleExecutingRedisCLICommand = React.useCallbackRef doExecuteRedisCLICommand

    React.useEffect (getTcpListeners >> Async.StartImmediate, [||])
    React.useEffect (getHackerNewsStories >> Async.StartImmediate, [||])

    Html.div [
      theme.nord
      prop.className "container mx-auto grid grid-cols-1 lg:grid-cols-2 py-8 gap-4"
      prop.children [
        Html.div [
          prop.className "flex flex-col gap-4"
          prop.children [
            DashboardComponents.TcpListeners(tcpListeners = tcpListeners)
            DashboardComponents.RedisCli(
              commands = redisCommands,
              onSubmitRedisCLICommand = handleExecutingRedisCLICommand
            )
          ]
        ]
        Html.div [
          prop.className "flex flex-col gap-4"
          prop.children [
            DashboardComponents.TopHackerNewsStories(hackerNewsStories = hackerNewsStories)
          ]
        ]
      ]
    ]
