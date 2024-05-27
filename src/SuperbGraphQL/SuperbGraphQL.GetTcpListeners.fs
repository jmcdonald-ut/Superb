[<RequireQualifiedAccess>]
module rec SuperbGraphQL.GetTcpListeners

type TcpListenerType = {
  command: string
  hosts: list<string>
  processId: string
  user: string
}

type Query = {
  tcpListeners: Option<list<Option<TcpListenerType>>>
}
