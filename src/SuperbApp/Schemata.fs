namespace SuperbApp

open HotChocolate
open HotChocolate.Types

module Schemata =
  type TcpListener = {
    ProcessId: string
    Command: string
    User: string
    Hosts: string list
  } with

    static member Default = {
      ProcessId = "<UNKNOWN>"
      Command = "<UNKNOWN>"
      User = "<UNKNOWN>"
      Hosts = []
    }

  type TcpListenerType(tcpListener: TcpListener) =
    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.ProcessId = tcpListener.ProcessId

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.Command = tcpListener.Command

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.User = tcpListener.User

    [<GraphQLType(typeof<NonNullType<ListType<NonNullType<StringType>>>>)>]
    member _.Hosts = tcpListener.Hosts
