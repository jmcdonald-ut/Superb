namespace SuperbApp.Tests

open NUnit.Framework
open SuperbApp.Schemata

module TestSchemata =
  [<SetUp>]
  let Setup () = ()

  [<Test>]
  let TestTcpListenerDefault () =
    let actual = TcpListener.Default

    let expected = {
      TcpListener.Hosts = []
      Command = "<UNKNOWN>"
      User = "<UNKNOWN>"
      ProcessId = "<UNKNOWN>"
    }

    Assert.That(actual, Is.EqualTo(expected))
