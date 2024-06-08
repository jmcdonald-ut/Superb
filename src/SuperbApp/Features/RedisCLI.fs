namespace SuperbApp.Features

open System

open SuperbApp.Helpers

module RedisCLI =
  // Reads all output of the executed command into a string.
  let private stringFromProcess (executedProcess: Diagnostics.Process) =
    executedProcess.StandardOutput.ReadToEnd()

  let send (command: string) =
    let executable =
      Processes.ExecutableProcess.Build(
        executableName = "redis-cli",
        arguments = sprintf "-e %s" command,
        redirectStandardOutput = true
      )

    Processes.executeCapture stringFromProcess executable
