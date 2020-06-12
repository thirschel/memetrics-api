<h1 align="center">A digitial-life viewing application</h1>

<h3 align="center">
  <a href="https://memetrics.net/">Visit MeMetrics</a>
</h3>

<h3 align="center">
  <a href="https://github.com/thirschel/memetrics-ui/blob/master/ARCHITECTURE.md">Architecture Diagram</a> |
  <a href="https://github.com/thirschel/memetrics-ui">UI</a> | 
  <a href="https://github.com/thirschel/memetrics-infrastructure">Infrastructure</a> |
  <a href="https://github.com/thirschel/memetrics-functions">Functions</a> |
  <a href="https://github.com/thirschel/memetrics-imessage-updater">iMessage Updater</a>
</h3>

This project was generated using the [CQRS Microservice Template](https://github.com/thirschel/dotnet-cqrs-microservice-template)!

## What is this?

This project contains the Api endpoints for MeMetrics. This web app serves up metric data and ingests new incoming data from the function app.
Due to large data scale of some of the date ranges, this app uses in-memory caching to handle serving the metrics. Once a metric is retrieved it is cached for 24 hours. This is because the function app runs every 24 hours new data is never entered mid day, thus allowing for the cache to be rather long lived.

## Technology / Methodology
- .NET Core 3.1
- CQRS
- Mediator
- Automapper
- Dapper
- Terraform
- Docker
- Azure Pipelines

## Setting up development environment 🛠

This project can be run using any apporach a normal .NET core application could be run (VS, VS Code, dotnet sdk) as well as using the Dockerfile to build an image.

The environment variables specified [here](https://github.com/thirschel/memetrics-api/blob/master/src/MeMetrics.Infrastructure/EnvironmentConfiguration.cs) need to be supplied (via `appsettings.json`, ENV variables, etc)