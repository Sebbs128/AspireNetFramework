# Aspire Orchestrating ASP.NET Proof of Concept

An experiment to use an ASP.NET MVC project with .NET Aspire. I'm hoping this might eventually form the basis of a library.

The Aspire AppHost orchestrates

- `coreApi`: an ASP.NET Core Web API
- `frameworkMvc`: an ASP.NET MVC project
  - the project is started via IIS Express, reading the config in `.vs\AspireNetFramework\config\appliationhost.config`
  - references to the service endpoints of `coreApi`
    - Aspire injects these to the IIS Express process as environment variables, which can be read via `ConfigurationManager` after setting up `Microsoft.Configuration.ConfigurationBuilders.EnvironmentConfigBuilder` in the `web.config`
- `coreMvc`: an ASP.NET Core MVC project
  - references to the service endpoints of `frameworkMvc`, for use with YARP
  - references to the service endpoionts of `coreApi` (although currently unused)

## Why???

For those of us on the ASP.NET->ASP.NET Core migration path (using [the incremental approach](https://learn.microsoft.com/en-us/aspnet/core/migration/inc/overview?view=aspnetcore-8.0)), the developer inner loop can be a bit rough. For a simpler solution of a database, web api, and frontend web app, config already gets a bit more complex (db conn string, web api URL, plus the legacy web projects now being proxied by the aspnetcore projects), plus we now have *four* projects we need to ensure Visual Studio starts. I think it would be great if we could make this easier, which (excluding the support for legacy ASP.NET projects) was a goal of Aspire as well.

## Current shortcomings

Some areas where work is needed:

- Visual Studio debugger isn't automatically attached to `frameworkMvc`
  - it can be attached manually though (Debug->Attach to Process...)
- the ports for `frameworkMvc` bypass the Aspire AppHost's proxy
- service endpoint resolution in `frameworkMvc` (in the `WeatherController`) is currently very ad-hoc, and needs to be developed further

## TODO

Aside from resolving shortcomings, other things to add for this experiment include

- Adding SQL Server to the AppHost, and resolving the connection string in `frameworkMvc`
