[<RequireQualifiedAccess>]
module rec SuperbGraphQL.GetHackerNewsStories

type Story = {
  by: Option<string>
  comments: Option<list<int>>
  title: Option<string>
  url: Option<string>
}

type Query = {
  hackerNewsStories: Option<list<Option<Story>>>
}
