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
  /// Fetches the top stories on Hacker News.
  /// </summary>
  /// <returns>
  /// A successful result with normalized stories; otherwise an error result
  /// with a list of errors.
  /// </returns>
  let getHackerNewsStories () =
    async {
      let fromOptionOrEmptyString = Option.defaultValue ""
      let fromOptionOrEmptyList = Option.defaultValue []

      let toHackerNewsStory (story: GetHackerNewsStories.Story) : HackerNewsStory = {
        storyId = story.storyId
        by = fromOptionOrEmptyString story.by
        title = fromOptionOrEmptyString story.title
        url = fromOptionOrEmptyString story.url
        comments = fromOptionOrEmptyList story.comments
        commentCount = story.commentCount
      }

      let intoNewListIfSome (story: GetHackerNewsStories.Story option) (newList: HackerNewsStory list) =
        match story with
        | Some gqlStory -> (toHackerNewsStory gqlStory) :: newList
        | None -> newList

      let foldIntoNormalizedList (stories: GetHackerNewsStories.Story option list) =
        List.foldBack intoNewListIfSome stories []

      match! client.GetHackerNewsStories() with
      | Ok { hackerNewsStories = Some(stories) } -> return stories |> foldIntoNormalizedList |> Ok
      | Ok _ -> return Ok []
      | Error reason -> return Error reason
    }

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
      | Ok { tcpListeners = None } -> return Ok []
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
