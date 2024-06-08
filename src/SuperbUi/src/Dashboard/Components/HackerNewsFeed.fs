namespace SuperbUi.Dashboard.Components

open Feliz
open Feliz.DaisyUI
open SuperbUi

[<RequireQualifiedAccess>]
module HackerNewsFeed =
  /// <summary>
  /// Presents a single Hacker News story as a table row.
  /// </summary>
  [<ReactComponent>]
  let HackerNewsStoryRow (story: SuperbGraphQL.GetHackerNewsStories.StoryType) =
    let userUrl (userId: string) =
      sprintf "https://news.ycombinator.com/user?id=%s" userId

    let commentsUrl (storyId: int) =
      sprintf "https://news.ycombinator.com/item?id=%d" storyId

    Html.tr [
      prop.className "align-top"
      prop.children [
        Html.td [ Components.ExternalLink(href = story.url, text = story.title) ]
        Html.td [ Components.ExternalLink(href = (userUrl story.by), text = story.by) ]
        Html.td [
          Components.ExternalLink(href = (commentsUrl story.storyId), text = story.commentCount.ToString())
        ]
      ]
    ]

  /// <summary>
  /// Presents top Hacker News stories.
  /// </summary>
  [<ReactComponent>]
  let DashboardModule () =
    let hackerNewsStoriesState = Hooks.useHackerNewsStories ()

    DashboardModule.ModuleWidget {
      moduleName = "hn-stories"
      title = "Hacker News"
      children = [
        Daisy.table [
          prop.className "table-sm"
          prop.children [
            Html.thead [ Html.tr [ Html.th "Title"; Html.th "Author"; Html.th "Comments" ] ]
            Html.tbody (Seq.map HackerNewsStoryRow hackerNewsStoriesState.data)
          ]
        ]
      ]
      errors = hackerNewsStoriesState.errors
    }
