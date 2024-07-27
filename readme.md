<p align="center">
  <a title="Project Logo">
    <img height="150" style="margin-top:15px" src="https://raw.githubusercontent.com/Advanced-Systems/assets/master/logos/svg/min/adv-logo.svg">
  </a>
</p>

<h1 align="center">Advanced Systems Core</h1>

[![Unit Tests](https://github.com/Advanced-Systems/core/actions/workflows/dotnet-tests.yml/badge.svg)](https://github.com/Advanced-Systems/core/actions/workflows/dotnet-tests.yml)
[![CodeQL](https://github.com/Advanced-Systems/core/actions/workflows/codeql.yml/badge.svg)](https://github.com/Advanced-Systems/core/actions/workflows/codeql.yml)
[![Docs](https://github.com/Advanced-Systems/core/actions/workflows/docs.yml/badge.svg)](https://github.com/Advanced-Systems/core/actions/workflows/docs.yml)

## About

General purpose library for building .NET projects.

## Developer Notes

Run the test suite:

```powershell
dotnet test .\AdvancedSystems.Core.Tests\ --no-logo
```

This project also uses stryker for mutation testing, which is setup to be installed with

```powershell
dotnet tool restore --configfile nuget.config
```

Run stryker locally:

```powershell
dotnet stryker
```

Build and serve documentation locally:

```powershell
docfx .\docs\docfx.json --serve
```
