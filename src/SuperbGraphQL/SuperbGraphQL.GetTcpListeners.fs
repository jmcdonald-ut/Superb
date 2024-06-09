[<RequireQualifiedAccess>]
module rec SuperbGraphQL.GetTcpListeners

type TcpListenerType = {
  command: string
  hosts: list<string>
  processId: string
  user: string
}

/// The root query type intended for use with GraphQL. Each member representsan available root field.
type Query = {
  tcpListeners: Option<list<Option<TcpListenerType>>>
}
