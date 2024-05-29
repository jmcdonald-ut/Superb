namespace SuperbUi

open SuperbGraphQL

module GraphQLClient =
  // fsharplint:disable RecordFieldNames
  type HackerNewsStory = {
    storyId: int
    by: string
    url: string
    title: string
    comments: int list
    commentCount: int
  }

  type TcpListener = GetTcpListeners.TcpListenerType
  // fsharplint:enable RecordFieldNames

  /// <summary>
  /// Shared GraphQL client.
  /// </summary>
  let client: SuperbGraphQLGraphqlClient =
    SuperbGraphQLGraphqlClient(url = "https://localhost:7011/graphql")

  /// <summary>
  /// Fetches visible TCP listeners from the GraphQL endpoint and normalizes the
  /// result.
  /// </summary>
  /// <returns>
  /// A successful result with a normalized TCP listener list; otherwise an
  /// error result with a list of errors.
  /// </returns>
  let getTcpListeners () =
    async {
      let intoNewListIfSomething (newList: TcpListener list) =
        function
        | Some listener -> listener :: newList
        | None -> newList

      let foldIntoNormalizedList = List.fold intoNewListIfSomething []

      match! client.GetTcpListeners() with
      | Ok { tcpListeners = Some(list) } -> return list |> foldIntoNormalizedList |> Ok
      | Ok { tcpListeners = None } -> return Error [ { message = "Nothing" } ]
      | Error reason -> return Error reason
    }

  let executeRedisCLICommand (command: string) =
    async {
      let input: ExecuteRedisCLICommand.InputVariables = { command = Some command }

      match! client.ExecuteRedisCLICommand(input) with
      | Ok { executeRedisCLICommand = Some(thing) } -> return Ok thing
      | Ok { executeRedisCLICommand = None } -> return Error "no response"
      | Error reason -> return Error(sprintf "%A" reason)
    }
