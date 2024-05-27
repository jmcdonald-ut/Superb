[<RequireQualifiedAccess>]
module rec SuperbGraphQL.GetTcpListeners

type TcpListener = {
  command: Option<string>
  hosts: Option<list<Option<string>>>
  processId: Option<string>
  user: Option<string>
}

type Query = {
  tcpListeners: Option<list<Option<TcpListener>>>
}
