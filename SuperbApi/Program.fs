namespace SuperbApi

#nowarn "20"

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

open SuperbApp

module Program =
  let exitCode = 0

  [<EntryPoint>]
  let main args =

    let builder = WebApplication.CreateBuilder(args)

    let configureCors (corsBuilder: Cors.Infrastructure.CorsPolicyBuilder) : unit =
      corsBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod() |> ignore

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
