namespace SuperbApi

#nowarn "20"

open System

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

open SuperbApp

module Server =
  let exitCode: int = 0

  let private configureCors (corsBuilder: Cors.Infrastructure.CorsPolicyBuilder) =
    corsBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod() |> ignore

  [<EntryPoint>]
  let bootstrap (args: string array) =
    let builder: WebApplicationBuilder = WebApplication.CreateBuilder(args = args)

    builder.Services.AddControllers()
    builder.Services.AddScoped<Query>()
    builder.Services.AddGraphQLServer().AddQueryType<Query>()

    let app = builder.Build()

    app.UseHttpsRedirection()
    app.UseAuthorization()
    app.UseCors(configureCors)

    app.MapControllers()
    app.MapGet("/", Func<string>(fun () -> "Welcome to Superb!")) |> ignore
    app.MapGraphQL()

    app.Run()

    exitCode
