# Superb

Is full stack F# superb? I'm still working that out.

### 📸 Progress!

[![screenshot of app](/doc/quick-preview-thumb.png?raw=true)](/doc/quick-preview.png)

## Table of Contents

1. **[Set Up](#set-up)**
2. **[Architecture](#architecture)**
3. **[Develop](#develop)**
   - [Build](#build)
      - API: `dotnet build` | `dotnet clean`
      - Client: `yarn build` | `yarn clean`
      - GraphQL: `dotnet snowflaqe --generate`
   - [Run](#run)
      - API: `dotnet run --project src/SuperbApi/SuperbApi.fsproj --launch-profile https:watch`
      - Client: `yarn start`
   - [Format](#format)
      - API: `dotnet fantomas src/**/*.fs`
   - [Analyze](#analyze)
      - API: `dotnet fsharplint lint Superb.sln`
   - [Audit](#audit)
      - API: `dotnet paket ...` (auditing occurs w/ normal usage)
      - Client: `yarn npm audit`
   - [Test](#test)
      - API: `dotnet test`

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

#### Build

- Build or clean app/API artifacts (as well as some client artifacts):

  ```sh
  dotnet build
  dotnet clean
  ```
- Build or clean client artifacts (note that this also builds any necessary/missing client .NET artifacts):

  ```sh
  yarn build
  yarn clean
  ```
- Generate GraphQL artifacts for the client:

  ```sh
  dotnet snowflaqe --generate
  ```
  - **NOTE:** This is necessary after updating the GraphQL schema or GraphQL operations.

Building or cleaning the app/api/client is generally not necessary in development. However, both actions can be useful, as a troubleshooting step, if anything is failing to run. It is necessary to build GraphQL artifacts for the client after updating the GraphQL schema/operations.

#### Run

- App/API (automatically rebuilds when relevant code changes):

  ```sh
  dotnet watch --project src/SuperbApi/SuperbApi.fsproj --launch-profile https
  ```
  - https://localhost:7011/graphql/ should open in the browser upon running this.
- Client (also rebuilds when relevant code changes):

  ```sh
  yarn start
  ```
  - Once `vite` finishes any set up, press `o` to open http://localhost:5173/ in the browser.

#### Format

- Format F# code with [Fantomas](https://fsprojects.github.io/fantomas/). There are editor integrations available to streamline this.

  ```sh
  dotnet fantomas **/*.fs
  ```

#### Analyze

- Fantomas does a great job formatting code automatically, but it doesn't cover more nuanced style issues. [FSharpLint](https://fsprojects.github.io/FSharpLint/) helps in this arena.

  ```sh
  dotnet fsharplint lint Superb.sln
  ```

#### Audit

- .NET dependencies are managed with Paket, and sourced from NuGet. Restoring/installing packages automatically scans for known vulnerabilities, and if any are found, reports them. The article [Auditing package dependencies for security vulnerabilities](https://learn.microsoft.com/en-us/nuget/concepts/auditing-packages) covers this in more detail.
- JS packages can be audited with:

  ```sh
  yarn npm audit
  ```

#### Test

- Run API tests:

  ```sh
  dotnet test
  ```
