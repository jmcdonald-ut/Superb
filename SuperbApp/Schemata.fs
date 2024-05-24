namespace SuperbApp.Schemata

type Process(description: string, port: string) =
  member this.Description = description
  member this.Port = port
