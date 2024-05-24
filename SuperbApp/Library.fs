namespace SuperbApp

open SuperbApp.Schemata

type Query() =
  member this.GetProcesses() = [ new Process("Fake Process", "3000") ]
