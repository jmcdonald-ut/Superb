namespace SuperbApp

open FSharp.Data

module News =
  [<Literal>]
  let topStoryIdsUrl: string = "https://hacker-news.firebaseio.com/v0/topstories.json"

  [<Literal>]
  let itemSample: string =
    """
    {"by":"signa11","descendants":2,"id":40464529,"kids":[40475098,40475088,40475078,40476894],"score":18,"time":1716543983,"title":"Willow Sideloading Protocol","type":"story","url":"https://willowprotocol.org/specs/sideloading"}
    {"by":"eranation","id":40483424,"parent":40483408,"text":"Good point, edited the question.","time":1716741364,"type":"comment"}
    {"by":"khjjjukykhhjy","dead":true,"id":40476894,"parent":40464529,"text":"[dead]","time":1716661654,"type":"comment"}
    {"deleted":true,"id":40475078,"parent":40464529,"time":1716645062,"type":"comment"}
    {"by":"Old_Thrashbarg","id":40481509,"score":1,"time":1716724948,"title":"UpCodes (YC S17) is hiring remote SWEs, PMs to automate construction compliance","type":"job","url":"https://up.codes/careers?utm_source=HN"}
    {"by":"alfiedotwtf","descendants":355,"id":30239441,"kids":[30241049,30241934],"parts":[30239442,30239443,30239444,30239445,30239446],"score":312,"text":"@bckr submitting this as an Ask HN[1], but I think we would all get better insight if this were a poll<p>[1] https:&#x2F;&#x2F;news.ycombinator.com&#x2F;item?id=30239283","time":1644200571,"title":"Poll: Do you prefer the office or work from home?","type":"poll"}
    {"by":"alfiedotwtf","id":30239444,"poll":30239441,"score":228,"text":"Work from home, prefer working from the office","time":1644200571,"type":"pollopt"}
    """

  type TopStoryIds = JsonProvider<topStoryIdsUrl>
  type ItemProvider = JsonProvider<itemSample, SampleIsList=true>
  type RawItem = ItemProvider.Root

  let private intoListOfNormalizedStories (item: ItemProvider.Root) (stories: Schemata.StoryType array) =
    match (item.Type, item.By, item.Title, item.Url) with
    | ("story", Some(by), Some(title), Some(url)) ->
      let story: Schemata.Story = {
        StoryId = item.Id
        By = by
        Title = title
        Url = url
        Comments = item.Kids
        CommentCount = Option.defaultValue 0 item.Descendants
      }

      Array.insertAt 0 (new Schemata.StoryType(story)) stories
    | _ -> stories

  let private makeItemUrl (id: int) =
    sprintf "https://hacker-news.firebaseio.com/v0/item/%d.json" id

  /// <summary>
  /// Loads the top stories from Hacker News.
  /// </summary>
  let loadTopStories () =
    async {
      let loadOne (id: int) =
        ItemProvider.AsyncLoad(uri = (makeItemUrl id))

      let! (topStoryIds: int array) = TopStoryIds.AsyncLoad(topStoryIdsUrl)
      let! (stories: RawItem array) = topStoryIds |> Array.take 50 |> Array.map loadOne |> Async.Parallel

      return Array.foldBack intoListOfNormalizedStories stories [||]
    }
