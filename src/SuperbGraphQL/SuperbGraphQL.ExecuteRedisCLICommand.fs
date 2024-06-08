[<RequireQualifiedAccess>]
module rec SuperbGraphQL.ExecuteRedisCLICommand

type InputVariables = { command: Option<string> }

type Query =
    { executeRedisCLICommand: Option<string> }
