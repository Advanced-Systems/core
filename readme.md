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

A general purpose library for building .NET projects. This package can be installed
from the public [NuGet Gallery](https://www.nuget.org/packages/AdvancedSystems.Core):

```powershell
dotnet add package AdvancedSystems.Core
```

Package consumers can also use the symbols published to nuget.org symbol server by adding <https://symbols.nuget.org/download/symbols>
to their symbol sources in Visual Studio, which allows stepping into package code in the Visual Studio debugger. See
[Specify symbol (.pdb) and source files in the Visual Studio debugger](https://learn.microsoft.com/en-us/visualstudio/debugger/specify-symbol-dot-pdb-and-source-files-in-the-visual-studio-debugger)
for details on that process.

## Developer Notes

Run test suite:

```powershell
dotnet test .\AdvancedSystems.Core.Tests\ --no-logo
```

In addition to unit testing, this project also uses stryker for mutation testing, which is setup to be installed with

```powershell
dotnet tool restore --configfile nuget.config
```

Run stryker locally:

```powershell
dotnet stryker
```

Build and serve documentation locally (`http://localhost:8080`):

```powershell
docfx .\docs\docfx.json --serve
```
