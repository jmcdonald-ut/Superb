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

  type Id = int
  type Author = string
  type Url = string
  type Title = string
  type Kids = int array

  type Story = {
    StoryId: Id
    By: Author
    Url: Url
    Title: Title
    Comments: Kids
    CommentCount: int
  }

  type StoryType(story: Story) =
    member _.StoryId = story.StoryId
    member _.CommentCount = story.CommentCount

    [<GraphQLNonNullType>]
    member _.Comments = story.Comments

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.By = story.By

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.Url = story.Url

    [<GraphQLType(typeof<NonNullType<StringType>>)>]
    member _.Title = story.Title
