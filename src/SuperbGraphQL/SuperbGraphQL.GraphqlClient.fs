namespace SuperbGraphQL

open Fable.SimpleHttp
open Fable.SimpleJson

type GraphqlInput<'T> = { query: string; variables: Option<'T> }
type GraphqlSuccessResponse<'T> = { data: 'T }
type GraphqlErrorResponse = { errors: ErrorType list }

type SuperbGraphQLGraphqlClient(url: string, headers: Header list) =
  /// <summary>Creates SuperbGraphQLGraphqlClient specifying list of headers</summary>
  /// <remarks>
  /// In order to enable all F# types serialization and deserialization, this client uses Fable.SimpleJson from <a href="https://github.com/Zaid-Ajaj/Fable.SimpleJson">Fable.SimpleJson</a>
  /// </remarks>
  /// <param name="url">GraphQL endpoint URL</param>
  new(url: string) = SuperbGraphQLGraphqlClient(url, [])

  member _.GetTables(input: GetTables.InputVariables) =
    async {
      let query =
        """
                query getTables($schemaName: String!) {
                  tables(schemaName: $schemaName) {
                    autoIncrement
                    avgRowLength
                    checksum
                    checkTime
                    createOptions
                    createTime
                    dataFree
                    dataLength
                    engine
                    indexLength
                    maxDataLength
                    rowFormat
                    tableCatalog
                    tableCollation
                    tableComment
                    tableName
                    tableRows
                    tableSchema
                    tableType
                    updateTime
                    version
                  }
                }
            """

      let! response =
        Http.request url
        |> Http.method POST
        |> Http.headers [ Headers.contentType "application/json"; yield! headers ]
        |> Http.content (
          BodyContent.Text(
            Json.serialize {
              query = query
              variables = Some input
            }
          )
        )
        |> Http.send

      match response.statusCode with
      | 200 ->
        let response =
          Json.parseNativeAs<GraphqlSuccessResponse<GetTables.Query>> response.responseText

        return Ok response.data

      | errorStatus ->
        let response = Json.parseNativeAs<GraphqlErrorResponse> response.responseText
        return Error response.errors
    }

  member _.GetTcpListeners() =
    async {
      let query =
        """
                query GetTcpListeners {
                  tcpListeners {
                    command
                    hosts
                    processId
                    user
                  }
                }
            """

      let! response =
        Http.request url
        |> Http.method POST
        |> Http.headers [ Headers.contentType "application/json"; yield! headers ]
        |> Http.content (BodyContent.Text(Json.serialize { query = query; variables = None }))
        |> Http.send

      match response.statusCode with
      | 200 ->
        let response =
          Json.parseNativeAs<GraphqlSuccessResponse<GetTcpListeners.Query>> response.responseText

        return Ok response.data

      | errorStatus ->
        let response = Json.parseNativeAs<GraphqlErrorResponse> response.responseText
        return Error response.errors
    }

  member _.GetHackerNewsStories() =
    async {
      let query =
        """
                query GetHackerNewsStories {
                  hackerNewsStories {
                    storyId
                    by
                    comments
                    commentCount
                    title
                    url
                  }
                }
            """

      let! response =
        Http.request url
        |> Http.method POST
        |> Http.headers [ Headers.contentType "application/json"; yield! headers ]
        |> Http.content (BodyContent.Text(Json.serialize { query = query; variables = None }))
        |> Http.send

      match response.statusCode with
      | 200 ->
        let response =
          Json.parseNativeAs<GraphqlSuccessResponse<GetHackerNewsStories.Query>> response.responseText

        return Ok response.data

      | errorStatus ->
        let response = Json.parseNativeAs<GraphqlErrorResponse> response.responseText
        return Error response.errors
    }

  member _.GetSchemata() =
    async {
      let query =
        """
                query getSchemata {
                  schemata {
                    catalogName
                    defaultCharacterSetName
                    defaultCollationName
                    defaultEncryption
                    schemaName
                  }
                }
            """

      let! response =
        Http.request url
        |> Http.method POST
        |> Http.headers [ Headers.contentType "application/json"; yield! headers ]
        |> Http.content (BodyContent.Text(Json.serialize { query = query; variables = None }))
        |> Http.send

      match response.statusCode with
      | 200 ->
        let response =
          Json.parseNativeAs<GraphqlSuccessResponse<GetSchemata.Query>> response.responseText

        return Ok response.data

      | errorStatus ->
        let response = Json.parseNativeAs<GraphqlErrorResponse> response.responseText
        return Error response.errors
    }

  member _.ExecuteRedisCLICommand(input: ExecuteRedisCLICommand.InputVariables) =
    async {
      let query =
        """
                mutation ExecuteRedisCLICommand($command: String) {
                  executeRedisCLICommand(command: $command)
                }
            """

      let! response =
        Http.request url
        |> Http.method POST
        |> Http.headers [ Headers.contentType "application/json"; yield! headers ]
        |> Http.content (
          BodyContent.Text(
            Json.serialize {
              query = query
              variables = Some input
            }
          )
        )
        |> Http.send

      match response.statusCode with
      | 200 ->
        let response =
          Json.parseNativeAs<GraphqlSuccessResponse<ExecuteRedisCLICommand.Query>> response.responseText

        return Ok response.data

      | errorStatus ->
        let response = Json.parseNativeAs<GraphqlErrorResponse> response.responseText
        return Error response.errors
    }
