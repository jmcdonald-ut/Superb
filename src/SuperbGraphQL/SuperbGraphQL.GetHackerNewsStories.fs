[<RequireQualifiedAccess>]
module rec SuperbGraphQL.GetHackerNewsStories

type StoryType = {
  storyId: int
  by: string
  comments: list<int>
  commentCount: int
  title: string
  url: string
}

/// The root query type intended for use with GraphQL. Each member representsan available root field.
type Query = {
  hackerNewsStories: Option<list<Option<StoryType>>>
}
