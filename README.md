# Superb

Exploring full stack F#.

## Setup / Build

Instructions are written while on macOS 14.5.

First, install any missing system dependencies:

1. `dotnet` - [Homebrew](https://formulae.brew.sh/cask/dotnet) | [Microsoft: Install .NET on macOS](https://learn.microsoft.com/en-us/dotnet/core/install/macos)
2. `asdf` - [asdf: Getting Started Guide](https://asdf-vm.com/guide/getting-started.html)

Next, install project dependencies:

```sh
# Install current nodejs dependency.
asdf install

# Restore dotnet tools and then install NuGet packages
# using Paket.
#
# See: https://fsprojects.github.io/Paket/index.html
dotnet tool restore
dotnet paket install

# Optional, but useful if you want to develop using SSL
# (https) locally. Accept/trust the generated development
# certificate if prompted.
#
# See: https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio-code#run-the-app
dotnet dev-certs https --trust

# Install NPM packages - the first two commands may not be
# necessary - listing pending further reading.
corepack enable
asdf reshim
yarn
```

## Running the App in Development

Running the API/GraphQL server:

```sh
# Run the API server w/o HTTPS. The development server
# should be accessible at http://localhost:5130. Note that
# the port may differ. Refer to command output.
dotnet run --project SuperbApi/SuperbApi.fsproj

# Run the API server w/ HTTPS. The development server
# should be accessible at https://localhost:7011 OR
# http://localhost:5130. Note that either port may differ.
# Refer to command output.
dotnet run --project SuperbApi/SuperbApi.fsproj --launch-profile https

# Use `dotnet watch` to run the API server while
# recompiling the code due to relevant file changes.
dotnet watch --project SuperbApi/SuperbApi.fsproj
dotnet watch --project SuperbApi/SuperbApi.fsproj --launch-profile https
```

**PRO TIP:** Visit https://localhost:7011/graphql to explore the GraphQL schema. Swap out the port as appropriate.

Running the [Feliz (React)](https://github.com/Zaid-Ajaj/Feliz) frontend:

```sh
yarn start
```
