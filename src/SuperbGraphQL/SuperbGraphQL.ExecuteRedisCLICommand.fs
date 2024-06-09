[<RequireQualifiedAccess>]
module rec SuperbGraphQL.ExecuteRedisCLICommand

type InputVariables = { command: Option<string> }

/// Root mutation type intended for use with GraphQL. Each member representsa mutation.
type Query = {
  executeRedisCLICommand: Option<string>
}
