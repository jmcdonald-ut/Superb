namespace SuperbApp

open System

open SuperbApp.Schemata

module TcpListeners =
  // Reads all output of the executed command into a string.
  let private stringFromProcess (executedProcess: Diagnostics.Process) =
    executedProcess.StandardOutput.ReadToEnd()

  // Generates a list extracted from the string.
  let private splitStringByNewLine (string: string) =
    "\n" |> string.Split |> Array.toList |> Ok

  // Supports pattern matching a string based on a prefix. A match binds the
  // rest of the string.
  let private (|Prefix|_|) (prefix: string) (subject: string) =
    if subject.StartsWith(prefix) then
      Some(subject.Substring(prefix.Length))
    else
      None

  // We run `lsof` with `-F` in order to simplify parsing the command's output.
  // This outputs one attribute per line, and the attribute is prefixed with the
  // key used e.g. `-F c` => `credis-server`. The process ID is always included
  // and prefixed with `p<PROCESS_ID>`. This assumes that each distinct entry's
  // first line will be `p<PROCESS_ID>`.
  let private mapParsedIntoTcpListener (list: TcpListener list) (item: string) =
    let (headListener, newListener, rest) =
      match list with
      | [] -> (TcpListener.Default, TcpListener.Default, [])
      | latest :: rest -> (latest, TcpListener.Default, rest)

    match item with
    | Prefix "p" value -> { newListener with ProcessId = value } :: list
    | Prefix "c" value -> { headListener with Command = value } :: rest
    | Prefix "L" value -> { headListener with User = value } :: rest
    | Prefix "n" value when not (List.contains value headListener.Hosts) ->
      let nextHosts = value :: headListener.Hosts
      { headListener with Hosts = nextHosts } :: rest
    | _ -> list

  // Folds the string list parsed out from running `ls ...` into a list of
  // TcpListener records. Maps the value back into a Result type in order to
  // support proper usage with `Result.bind`.
  let private foldParsedIntoTcpListener (from: string list) =
    from |> List.fold mapParsedIntoTcpListener [] |> Ok

  /// <summary>
  /// Lists all visible TCP listeners bound to localhost.
  /// </summary>
  /// <returns>
  /// A result holding the TCP listeners if successful; otherwise an error with
  /// a brief message.
  /// </returns>
  let all () =
    let executable =
      Processes.ExecutableProcess.Build(
        executableName = "lsof",
        arguments = "-PM -i tcp@localhost -F cnL",
        redirectStandardOutput = true
      )

    executable
    |> Processes.executeCapture stringFromProcess
    |> Result.bind splitStringByNewLine
    |> Result.bind foldParsedIntoTcpListener
