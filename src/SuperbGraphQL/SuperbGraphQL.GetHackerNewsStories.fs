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

type Query = {
  hackerNewsStories: Option<list<Option<StoryType>>>
}
