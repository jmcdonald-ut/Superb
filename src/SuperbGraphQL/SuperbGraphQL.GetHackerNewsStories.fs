[<RequireQualifiedAccess>]
module rec SuperbGraphQL.GetHackerNewsStories

type Story = {
  storyId: int
  by: Option<string>
  comments: Option<list<int>>
  commentCount: int
  title: Option<string>
  url: Option<string>
}

type Query = {
  hackerNewsStories: Option<list<Option<Story>>>
}
