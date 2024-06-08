namespace SuperbUi.Dashboard.Components

open Feliz
open Feliz.DaisyUI
open SuperbUi

[<RequireQualifiedAccess>]
module RedisCLI =
  [<ReactComponent>]
  let RedisInputOutputEntry (inputOutput: RedisInputOutput) =
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
  let DashboardModule () =
    let redisCLIState = Hooks.useRedisCLI ()

    let nested = redisCLIState.executed |> Seq.map RedisInputOutputEntry |> Seq.toList

    let textInput =
      Html.pre [
        prop.custom ("data-prefix", "$")
        prop.children [
          Html.form [
            prop.className "inline"
            prop.onSubmit (fun ev ->
              ev.preventDefault () |> ignore
              ev.stopPropagation () |> ignore
              redisCLIState.executeRedisCLICommand ()
              redisCLIState.setCommand "")
            prop.children [
              Daisy.input [
                input.ghost
                prop.type'.text
                prop.className
                  "inline bg-transparent focus:border-none focus:outline-none text-primary-content focus:text-primary-content border-none py-0 px-0 h-auto"
                prop.onTextChange redisCLIState.setCommand
                prop.value redisCLIState.command
                prop.placeholder "Type Command"
              ]
            ]
          ]
        ]
      ]

    DashboardModule.ModuleWidget {
      moduleName = "redis-cli"
      title = "Redis CLI"
      children = [
        Daisy.mockupCode [
          Html.div [ prop.className "flex flex-col-reverse"; prop.children nested ]
          textInput
        ]
      ]
      errors = redisCLIState.errors
    }
