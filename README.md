# Superb

Is full stack F# superb?

## Table of Contents

1. **[Set Up](#set-up)**
2. **[Architecture](#architecture)**
2. **[Develop](#develop)**
   - [Run](#run)
   - [Format](#format)
   - [Analyze](#analyze)
   - [Audit](#audit)
   - [Test](#test)

## Set Up

Instructions written on a machine running macOS 14.5.

1. Install system dependencies necessary to build/run the app.

   - `dotnet` - [Homebrew](https://formulae.brew.sh/cask/dotnet) | [Microsoft: Install .NET on macOS](https://learn.microsoft.com/en-us/dotnet/core/install/macos)
   - `asdf` - [asdf: Getting Started Guide](https://asdf-vm.com/guide/getting-started.html)
2. Next, add the `nodejs` plugin for `asdf`, and then install `nodejs` itself.

   ```sh
   asdf plugin add nodejs
   asdf install
   ```
3. After installing system dependencies, and the `asdf` plugin, install runtime/project dependencies. Restore dotnet tools, install NuGet packages using [Paket](https://fsprojects.github.io/Paket/index.html), and then install NPM packages.

   ```sh
   dotnet tool restore
   dotnet paket install

   # The first two commands may not be necessary - listing
   # pending further reading.
   corepack enable
   asdf reshim
   yarn
   ```
4. Finally, take the optional step of generating dev certificates to support developing with SSL (https) locally. Accept/trust the generated development certificate if prompted. Find more on this in an [ASP.net Core Web API tutorial](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio-code#run-the-app).

   ```sh
   dotnet dev-certs https --trust
   ```

## Architecture

Superb is broken into four projects.

1. **SuperbApp** (`src/SuperbApp/SuperbApp.fsproj`)
   - Brain of Superb.
   - Provides primary types, data, and business logic.
   - No dependencies on other projects.
2. **SuperbApi** (`src/SuperbApi/SuperbApi.fsproj`)
   - Server/API for Superb.
   - Provides a GraphQL endpoint.
   - Depends on `SuperbApp`.
3. **SuperbGraphQL** (`src/SuperbGraphQL/SuperbGraphQL.fsproj`)
   - Queries, mutations, and types for GraphQL consumption on the frontend.
   - Besides `.graphql` queries and mutations, the code  in this project is mostly generated.
   - No dependencies on other projects.
3. **SuperbUi** (`src/SuperbUi/SuperbUi.fsproj`)
   - Frontend UI for Superb.
   - React application, powered by Feliz and Fable.
   - Depends on `SuperbGraphQL`. Implicitly depends on `SuperbApp` and `SuperbApi`.

## Develop

#### Run

The server and frontend run separately. Run the server app in "watch" mode so file changes automatically trigger a recompile. **PRO TIP:** Visit https://localhost:7011/graphql to explore the GraphQL schema.

```sh
# ## Running the server
#
# Omit `--launch-profile https` to run the app without
# HTTPS. Swap `watch` with `run` to opt-out of triggering
# recompilation when running the application.
#
# - HTTP URL: http://localhost:5130
# - HTTPS URL: https://localhost:7011
dotnet watch --project src/SuperbApi/SuperbApi.fsproj --launch-profile https

# ## Running the frontend
#
# `dotnet` is used behind the scenes to compile
# Felize/Fable. As noted earlier, this supports hot code
# reloading out of the box.
yarn start
```

#### Format

Format F# code with [Fantomas](https://fsprojects.github.io/fantomas/). There are editor integrations available to streamline this.

```sh
dotnet fantomas **/*.fs
```

#### Analyze

Fantomas does a great job formatting code automatically, but it doesn't cover more nuanced style issues. [FSharpLint](https://fsprojects.github.io/FSharpLint/) helps in this arena.

```sh
dotnet fsharplint lint Superb.sln
```

#### Audit

.NET dependencies are managed with Paket, and sourced from NuGet. Restoring/installing packages automatically scans for known vulnerabilities, and if any are found, reports them. The article [Auditing package dependencies for security vulnerabilities](https://learn.microsoft.com/en-us/nuget/concepts/auditing-packages) covers this in more detail.

JS packages can be audited with:

```sh
yarn npm audit
```

#### Test

```sh
dotnet test
```
