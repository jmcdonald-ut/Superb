namespace SuperbUi.Dashboard

module Types =
  type HackerNewsStory = SuperbGraphQL.GetHackerNewsStories.StoryType

  type RedisIOEntry = {
    didFail: bool
    input: string
    output: string
  }

  type RedisCLIHook = {
    command: string
    errors: string list
    executed: RedisIOEntry list
    executeRedisCLICommand: unit -> unit
    setCommand: string -> unit
  }
