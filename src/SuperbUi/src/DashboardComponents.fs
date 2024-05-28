namespace SuperbUi

open Feliz
open Feliz.DaisyUI

type RedisInputOutput = {
  didFail: bool
  input: string
  output: string
}

type DashboardComponents() =
  /// <summary>
  /// Renders a standalone dashboard module.
  /// </summary>
  [<ReactComponent>]
  static member DashboardModule(moduleName: string, title: string, children: ReactElement seq) =
    Daisy.card [
      card.bordered
      prop.children [ Daisy.cardBody [ Daisy.cardTitle title; Html.div children ] ]
    ]

  /// <summary>
  /// Presents a single TCP listener as a table row.
  /// </summary>
  [<ReactComponent>]
  static member TcpRow(tcpListener: GraphQLClient.TcpListener) =
    Html.tr [
      prop.className "align-top"
      prop.children [
        Html.td [ prop.className "align-top"; prop.text tcpListener.processId ]
        Html.td [ prop.className "align-top"; prop.text tcpListener.user ]
        Html.td [ prop.className "align-top"; prop.text tcpListener.command ]
        Html.td [
          prop.className "align-top"
          prop.children [ Html.ul (Seq.map Components.ListItem tcpListener.hosts) ]
        ]
      ]
    ]

  /// <summary>
  /// Presents visible TCP listeners in a table.
  /// </summary>
  [<ReactComponent>]
  static member TcpListeners(tcpListeners: GraphQLClient.TcpListener seq) =
    let toTcpRow (tcpListener: GraphQLClient.TcpListener) =
      DashboardComponents.TcpRow(tcpListener = tcpListener)

    DashboardComponents.DashboardModule(
      moduleName = "tcp-listeners",
      title = "TCP Listeners",
      children = [
        Daisy.table [
          Html.thead [
            Html.tr [ Html.th "Process"; Html.th "User"; Html.th "Command"; Html.th "Hosts" ]
          ]
          Html.tbody (Seq.map toTcpRow tcpListeners)
        ]
      ]
    )

  /// <summary>
  /// Presents a single Hacker News story as a table row.
  /// </summary>
  [<ReactComponent>]
  static member HackerNewsStoryRow(story: GraphQLClient.HackerNewsStory) =
    let userUrl (userId: string) =
      sprintf "https://news.ycombinator.com/user?id=%s" userId

    let commentsUrl (storyId: int) =
      sprintf "https://news.ycombinator.com/item?id=%d" storyId

    Html.tr [
      prop.className "align-top"
      prop.children [
        Html.td [ Components.ExternalLink(href = story.url, text = story.title) ]
        Html.td [ Components.ExternalLink(href = (userUrl story.by), text = story.by) ]
        Html.td [
          Components.ExternalLink(href = (commentsUrl story.storyId), text = story.commentCount.ToString())
        ]
      ]
    ]

  /// <summary>
  /// Presents top Hacker News stories.
  /// </summary>
  [<ReactComponent>]
  static member TopHackerNewsStories(hackerNewsStories: GraphQLClient.HackerNewsStory seq) =
    let toHackerNewsStoryRow (story: GraphQLClient.HackerNewsStory) =
      DashboardComponents.HackerNewsStoryRow(story = story)

    DashboardComponents.DashboardModule(
      moduleName = "hn-stories",
      title = "Hacker News",
      children = [
        Daisy.table [
          Html.thead [ Html.tr [ Html.th "Title"; Html.th "Author"; Html.th "Comments" ] ]
          Html.tbody (Seq.map toHackerNewsStoryRow hackerNewsStories)
        ]
      ]
    )

  static member RedisInputOutputEntry(inputOutput: RedisInputOutput) =
    let outputClass =
      if inputOutput.didFail then
        "bg-error text-error-content"
      else
        ""

    React.fragment [
      Html.pre [
        prop.custom ("data-prefix", ">")
        prop.className outputClass
        prop.text inputOutput.output
      ]
      Html.pre [ prop.custom ("data-prefix", "$"); prop.text inputOutput.input ]
    ]

  /// <summary>
  /// Redis CLI because why not?
  /// </summary>
  [<ReactComponent>]
  static member RedisCli(commands: RedisInputOutput seq, onSubmitRedisCLICommand) =
    let (value, setValue) = React.useState ("")

    let nested =
      commands |> Seq.map DashboardComponents.RedisInputOutputEntry |> Seq.toList

    let textInput =
      Html.pre [
        prop.custom ("data-prefix", "$")
        prop.children [
          Html.form [
            prop.className "inline"
            prop.onSubmit (fun ev ->
              ev.preventDefault () |> ignore
              ev.stopPropagation () |> ignore
              setValue ""
              onSubmitRedisCLICommand value)
            prop.children [
              Daisy.input [
                input.ghost
                prop.type'.text
                prop.className "inline border-none py-0 px-0 h-auto"
                prop.onTextChange setValue
                prop.value value
                prop.placeholder "Type Command"
              ]
            ]
          ]
        ]
      ]

    DashboardComponents.DashboardModule(
      moduleName = "redis-cli",
      title = "Redis CLI",
      children = [
        Daisy.mockupCode [
          Html.div [ prop.className "flex flex-col-reverse"; prop.children nested ]
          textInput
        ]
      ]
    )
