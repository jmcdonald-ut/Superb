namespace SuperbUi.Dashboard

open Feliz
open Feliz.DaisyUI

open SuperbUi.Dashboard.Hooks
open SuperbUi.Dashboard.Types
open SuperbUi.MySQLClient.Hooks
open SuperbUi.MySQLClient.Types
open SuperbUi.Shared.BaseComponents
open SuperbUi.Shared.Types

[<AutoOpen>]
module private InternalHelpers =
  let userUrl (userId: string) =
    sprintf "https://news.ycombinator.com/user?id=%s" userId

  let commentsUrl (storyId: int) =
    sprintf "https://news.ycombinator.com/item?id=%d" storyId

[<AutoOpen>]
module private InternalDashboardComponents =
  [<ReactComponent>]
  let DashboardTable (children: ReactElement seq) =
    Html.div [
      prop.className "overflow-x-auto"
      prop.children [ Daisy.table [ prop.className "min-w-full table-sm"; prop.children children ] ]
    ]

  [<ReactComponent>]
  let SkeletonDashboardTableCell (index) =
    Html.td [
      prop.key (sprintf "skeleton-cell-%d" index)
      prop.children (Daisy.skeleton [ prop.className "min-w-20 max-w-[75%] h-4" ])
    ]

  let buildSkeletonTableRows rowCount columnCount =
    let columns = List.init columnCount SkeletonDashboardTableCell
    List.init rowCount (fun _ -> Html.tr columns)

  [<ReactComponent>]
  let DashboardModule (title: string) (children: ReactElement seq) (maybeErrors: string list option) =
    let renderedErrors = maybeErrors |> Option.defaultValue [] |> ErrorAlert

    Daisy.card [
      card.bordered
      prop.children [
        Daisy.cardBody [
          Html.div [
            prop.className "flex place-content-between align-items-end"
            prop.children [ Daisy.cardTitle [ prop.className "font-black"; prop.text title ] ]
          ]
          Html.div [
            prop.className "flex flex-col gap-2"
            prop.children (Seq.insertAt 0 renderedErrors children)
          ]
        ]
      ]
    ]

  [<ReactComponent>]
  let HackerNewsStoryRow (story: HackerNewsStory) =
    Html.tr [
      prop.className "align-top"
      prop.children [
        Html.td [ ExternalLink(story.url, story.title) ]
        Html.td [ ExternalLink((userUrl story.by), story.by) ]
        Html.td [ ExternalLink((commentsUrl story.storyId), story.commentCount.ToString()) ]
      ]
    ]

  [<ReactComponent>]
  let RedisCommandInput (command: string) (onCommandChange: string -> unit) (onExecuteCommand: unit -> unit) =
    let handleSubmit (ev: Browser.Types.Event) =
      ev.preventDefault () |> ignore
      ev.stopPropagation () |> ignore
      onExecuteCommand ()
      onCommandChange ""

    Html.pre [
      prop.custom ("data-prefix", "$")
      prop.children [
        Html.form [
          prop.className "inline"
          prop.onSubmit handleSubmit
          prop.children [
            Daisy.input [
              input.ghost
              prop.type'.text
              prop.className
                "inline bg-transparent focus:border-none focus:outline-none text-primary-content focus:text-primary-content border-none py-0 px-0 h-auto"
              prop.onTextChange onCommandChange
              prop.value command
              prop.placeholder "Type Command"
            ]
          ]
        ]
      ]
    ]

  [<ReactComponent>]
  let RedisIOEntryRow (inputOutput: RedisIOEntry) =
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

  [<ReactComponent>]
  let SchemaRow (schema: Schema) =
    Html.tr [
      prop.className "align-top"
      prop.children [
        Html.td [ prop.className "align-top"; prop.text schema.schemaName ]
        Html.td [ prop.className "align-top"; prop.text schema.catalogName ]
        Html.td [ prop.className "align-top"; prop.text schema.defaultCharacterSetName ]
        Html.td [ prop.className "align-top"; prop.text schema.defaultCollationName ]
        Html.td [ prop.className "align-top"; prop.text schema.defaultEncryption ]
      ]
    ]

  [<ReactComponent>]
  let TcpRow (tcpListener: SuperbGraphQL.GetTcpListeners.TcpListenerType) =
    Html.tr [
      prop.className "align-top"
      prop.children [
        Html.td [ prop.className "align-top"; prop.text tcpListener.processId ]
        Html.td [ prop.className "align-top"; prop.text tcpListener.user ]
        Html.td [ prop.className "align-top"; prop.text tcpListener.command ]
        Html.td [
          prop.className "align-top"
          prop.children [ Html.ul (Seq.map ListItem tcpListener.hosts) ]
        ]
      ]
    ]

/// Provides components for all dashboard modules.
module Components =
  /// Displays the latest (top) Hacker News articles.
  [<ReactComponent>]
  let HackerNewsDashboardModule () =
    let (loadStatus, stories, _prior, maybeErrors) = useHackerNewsStories ()

    let tableRows =
      match loadStatus with
      | IsLoading
      | HasNotStarted -> buildSkeletonTableRows 50 3
      | _anythingElse -> List.map HackerNewsStoryRow stories

    let children = [
      DashboardTable [
        Html.thead [
          Html.tr [
            Html.th [ prop.className "min-w-80"; prop.text "Title" ]
            Html.th [ prop.className "min-w-40"; prop.text "Author" ]
            Html.th [ prop.className "min-w-24"; prop.text "Comments" ]
          ]
        ]
        Html.tbody tableRows
      ]
    ]

    DashboardModule "Hacker News" children maybeErrors

  /// Displays visible MySQL databases.
  [<ReactComponent>]
  let MySQLDashboardModule () =
    let (loadStatus, schemas, _priorSchemas, maybeErrors) = useSchemata ()

    let tableRows =
      match loadStatus with
      | IsLoading
      | HasNotStarted -> buildSkeletonTableRows 10 5
      | _anythingElse -> List.map SchemaRow schemas

    let children = [
      DashboardTable [
        Html.thead [
          Html.tr [
            Html.th [ prop.className "min-w-64"; prop.text "Schema" ]
            Html.th [ prop.className "min-w-24"; prop.text "Catalog" ]
            Html.th [ prop.className "min-w-40"; prop.text "Default Character Set" ]
            Html.th [ prop.className "min-w-52"; prop.text "Default Collation" ]
            Html.th "Default Encryption"
          ]
        ]
        Html.tbody tableRows
      ]
    ]

    DashboardModule "MySQL" children maybeErrors

  /// Provides a Redis "CLI" where commands are forwarded to the server and
  /// responses are displayed.
  [<ReactComponent>]
  let RedisDashboardModule () =
    let redisCLIState = useRedisCLI ()
    let executeCommand = redisCLIState.executeRedisCLICommand
    let command = redisCLIState.command
    let setCommand = redisCLIState.setCommand
    let ioRows = List.map RedisIOEntryRow redisCLIState.executed

    let children = [
      Daisy.mockupCode [
        Html.div [ prop.className "flex flex-col-reverse"; prop.children ioRows ]
        RedisCommandInput command setCommand executeCommand
      ]
    ]

    DashboardModule "Redis CLI" children (Some redisCLIState.errors)

  /// Presents visible/active TCP listeners.
  [<ReactComponent>]
  let TcpListenersDashboardModule () =
    let (loadStatus, listeners, _prior, maybeErrors) = useTcpListeners ()

    let tableRows =
      match loadStatus with
      | IsLoading
      | HasNotStarted -> buildSkeletonTableRows 5 4
      | _anythingElse -> List.map TcpRow listeners

    let children = [
      DashboardTable [
        Html.thead [
          Html.tr [ Html.th "Process"; Html.th "User"; Html.th "Command"; Html.th "Hosts" ]
        ]
        Html.tbody tableRows
      ]
    ]

    DashboardModule "TCP Listeners" children maybeErrors
