namespace SuperbUi

open SuperbGraphQL

module GraphQLClient =
  // fsharplint:disable RecordFieldNames
  type TcpListener = {
    command: string
    hosts: string list
    processId: string
    user: string
  }
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
      let fromOptionOrUnknownString = Option.defaultValue "<UNKNOWN>"
      let fromOptionOrEmptyList = Option.defaultValue []
      let mapOptionsToUnknown = List.map fromOptionOrUnknownString

      let toTcpListener (listener: GetTcpListeners.TcpListener) : TcpListener = {
        command = fromOptionOrUnknownString listener.command
        hosts = listener.hosts |> fromOptionOrEmptyList |> mapOptionsToUnknown
        processId = fromOptionOrUnknownString listener.processId
        user = fromOptionOrUnknownString listener.user
      }

      let intoNewListIfSomething (newList: TcpListener list) =
        function
        | Some listener -> (toTcpListener listener) :: newList
        | None -> newList

      let foldIntoNormalizedList = List.fold intoNewListIfSomething []

      match! client.GetTcpListeners() with
      | Ok { tcpListeners = Some(list) } -> return list |> foldIntoNormalizedList |> Ok
      | Ok { tcpListeners = None } -> return Ok []
      | Error reason -> return Error reason
    }
