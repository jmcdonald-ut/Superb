namespace App

open Feliz
open Feliz.Router

open SuperbGraphQL

module Helpers =
  let client =
    new SuperbGraphQL.SuperbGraphQLGraphqlClient("https://localhost:7011/graphql")

  let orEmptyString = Option.defaultValue ""

  let intoNewListIfSomething (newList: GetTcpListeners.TcpListener list) =
    function
    | Some listener -> listener :: newList
    | None -> newList

  let extractNextTcpListeners =
    function
    | Some list -> List.fold intoNewListIfSomething [] list
    | None -> []

type Components =
  /// <summary>
  /// The simplest possible React component.
  /// Shows a header with the text Hello World
  /// </summary>
  [<ReactComponent>]
  static member HelloWorld() = Html.h1 "Hello World"

  /// <summary>
  /// A stateful React component that maintains a counter
  /// </summary>
  [<ReactComponent>]
  static member Counter() =
    let (count, setCount) = React.useState (0)

    let (tcpListeners, setTcpListeners) =
      React.useState<GetTcpListeners.TcpListener list> ([])

    let getTcpListeners () =
      async {
        let! result = Helpers.client.GetTcpListeners()

        match result with
        | Ok { tcpListeners = optional } -> optional |> Helpers.extractNextTcpListeners |> setTcpListeners |> ignore
        | Error _ -> ()
      }

    React.useEffect (getTcpListeners >> Async.StartImmediate, [||])

    let listItems =
      List.map (fun (x: GetTcpListeners.TcpListener) -> Html.li (Helpers.orEmptyString x.command)) tcpListeners

    Html.div [
      Html.h1 count
      Html.button [ prop.onClick (fun _ -> setCount (count + 1)); prop.text "Increment" ]
      Html.ul listItems
    ]

  /// <summary>
  /// A React component that uses Feliz.Router
  /// to determine what to show based on the current URL
  /// </summary>
  [<ReactComponent>]
  static member Router() =
    let (currentUrl, updateUrl) = React.useState (Router.currentUrl ())

    React.router [
      router.onUrlChanged updateUrl
      router.children [
        match currentUrl with
        | [] -> Html.h1 "Index"
        | [ "hello" ] -> Components.HelloWorld()
        | [ "counter" ] -> Components.Counter()
        | otherwise -> Html.h1 "Not found"
      ]
    ]
