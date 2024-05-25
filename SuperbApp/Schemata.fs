namespace SuperbApp

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
