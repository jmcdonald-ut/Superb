namespace SuperbApp.Helpers

open System.Diagnostics

module Processes =
  type ExecutableProcess = {
    StartInfo: ProcessStartInfo
    Executable: Process
    Inspect: string
  } with

    static member Build
      (
        executableName: string,
        arguments: string,
        ?workingDirectory: string,
        ?useShellExecute: bool,
        ?redirectStandardOutput: bool,
        ?redirectStandardInput: bool
      ) =
      let startInfo: ProcessStartInfo =
        ProcessStartInfo(
          FileName = executableName,
          Arguments = arguments,
          UseShellExecute = (defaultArg useShellExecute false),
          RedirectStandardInput = (defaultArg redirectStandardInput false),
          RedirectStandardOutput = (defaultArg redirectStandardOutput false),
          WorkingDirectory = (defaultArg workingDirectory "")
        )

      let executable: Process = new Process(StartInfo = startInfo)

      {
        StartInfo = startInfo
        Executable = executable
        Inspect = sprintf "`%s %s`" executableName arguments
      }

  let resultOfExit (executableProcess: ExecutableProcess) (forward: 'Result) =
    executableProcess.Executable.WaitForExit()

    match executableProcess.Executable.ExitCode with
    | 0 -> Ok(forward)
    | errorCode -> Error(sprintf "%s failed with error code %i" executableProcess.Inspect errorCode)

  let executeTap (callback: (Process) -> 'Result) (executableProcess: ExecutableProcess) =
    match executableProcess.Executable.Start() with
    | false -> Error(sprintf "%s failed to start" executableProcess.Inspect)
    | true ->
      callback (executableProcess.Executable) |> ignore
      resultOfExit executableProcess executableProcess

  let executeCapture (callback: (Process) -> 'Result) (executableProcess: ExecutableProcess) =
    match executableProcess.Executable.Start() with
    | false -> Error(sprintf "%s failed to start" executableProcess.Inspect)
    | true -> executableProcess.Executable |> callback |> resultOfExit executableProcess

  let execute (executableProcess: ExecutableProcess) =
    match executableProcess.Executable.Start() with
    | false -> Error(sprintf "%s failed to start" executableProcess.Inspect)
    | true -> resultOfExit executableProcess executableProcess
