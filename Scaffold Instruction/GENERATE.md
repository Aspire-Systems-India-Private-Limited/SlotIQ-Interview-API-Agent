# Solution Generation Guide

This guide explains how to generate a new solution using the scaffold configuration.

## Prerequisites

1. .NET 9.0 SDK installed
2. PowerShell 7.0 or later
3. Updated `scaffold.config.json` file

## Option 1: Using PowerShell Script

Save this script as `New-ScaffoldSolution.ps1`:

```powershell
[CmdletBinding()]
param(
    [Parameter()]
    [string]$ConfigPath = "scaffold.config.json"
)

# Validate config file exists
if (-not (Test-Path $ConfigPath)) {
    throw "Configuration file not found at: $ConfigPath"
}

# Read and parse configuration
$config = Get-Content $ConfigPath -Raw | ConvertFrom-Json

# Extract variables
$variables = $config.variables
$customization = $config.customization
$database = $config.database

# Create solution directory
$solutionPath = Join-Path $PWD $variables.SolutionName
New-Item -ItemType Directory -Path $solutionPath -Force

# Create solution file
Set-Location $solutionPath
dotnet new sln --name $variables.SolutionName

# Create projects
$projects = @(
    "classlib" # Common
    "classlib" # Data
    "classlib" # Logic
    "webapi"   # API
    "classlib" # Integration
    "xunit"    # UnitTests
)

$projectNames = @(
    "$($variables.ProjectPrefix).Common"
    "$($variables.ProjectPrefix).Data"
    "$($variables.ProjectPrefix).Logic"
    "$($variables.ProjectPrefix).API"
    "$($variables.ProjectPrefix).Integration"
    "$($variables.ProjectPrefix).UnitTests"
)

for ($i = 0; $i -lt $projects.Length; $i++) {
    $template = $projects[$i]
    $name = $projectNames[$i]
    
    Write-Host "Creating project: $name"
    dotnet new $template -n $name
    dotnet sln add $name
}

# Update project properties
$projects | ForEach-Object {
    $projFile = Join-Path $_ "$_.csproj"
    if (Test-Path $projFile) {
        # Update target framework
        (Get-Content $projFile) -replace '<TargetFramework>.*</TargetFramework>', "<TargetFramework>$($variables.DotNetVersion)</TargetFramework>" | Set-Content $projFile
        
        # Add nullable enable
        (Get-Content $projFile) -replace '<PropertyGroup>', "<PropertyGroup>`n    <Nullable>enable</Nullable>" | Set-Content $projFile
    }
}

# Add project references
dotnet add "$($variables.ProjectPrefix).Data" reference "$($variables.ProjectPrefix).Common"
dotnet add "$($variables.ProjectPrefix).Logic" reference "$($variables.ProjectPrefix).Data" "$($variables.ProjectPrefix).Integration" "$($variables.ProjectPrefix).Common"
dotnet add "$($variables.ProjectPrefix).API" reference "$($variables.ProjectPrefix).Logic" "$($variables.ProjectPrefix).Data" "$($variables.ProjectPrefix).Common"
dotnet add "$($variables.ProjectPrefix).Integration" reference "$($variables.ProjectPrefix).Common"
dotnet add "$($variables.ProjectPrefix).UnitTests" reference "$($variables.ProjectPrefix).API" "$($variables.ProjectPrefix).Logic" "$($variables.ProjectPrefix).Data" "$($variables.ProjectPrefix).Common" "$($variables.ProjectPrefix).Integration"

# Add NuGet packages
$packages = @{
    "Common" = @(
        "Microsoft.Extensions.DependencyInjection"
        "Microsoft.Extensions.Logging.Abstractions"
    )
    "Data" = @(
        "Dapper,$($variables.DapperVersion)"
        "Microsoft.Data.SqlClient"
    )
    "Logic" = @(
        "AutoMapper,$($variables.MapperVersion)"
        "FluentValidation,$($variables.ValidatorVersion)"
    )
    "API" = @(
        "Swashbuckle.AspNetCore"
        "Serilog.AspNetCore"
    )
    "UnitTests" = @(
        "xunit,$($variables.TestingVersion)"
        "Moq"
        "FluentAssertions"
    )
}

foreach ($project in $packages.Keys) {
    $projectPath = "$($variables.ProjectPrefix).$project"
    $packages[$project] | ForEach-Object {
        $package = $_
        if ($package -match ',') {
            $name, $version = $package -split ','
            dotnet add $projectPath package $name -v $version
        } else {
            dotnet add $projectPath package $package
        }
    }
}

# Create basic folder structure
$folders = @(
    "Common/Constants",
    "Common/Enums",
    "Common/Helpers",
    "Common/Models",
    "Data/Entities",
    "Data/Repositories/Contracts",
    "Data/Sql",
    "Logic/Commands",
    "Logic/Queries",
    "Logic/Handlers/Commands",
    "Logic/Handlers/Queries",
    "Logic/Dtos",
    "Logic/Mapping",
    "API/Configuration",
    "API/Endpoints",
    "API/HealthChecks",
    "API/Middleware",
    "API/Models",
    "Integration/Clients/Interfaces",
    "Integration/Clients/Implementations",
    "Integration/Configuration",
    "UnitTests/Tests/API",
    "UnitTests/Tests/Logic",
    "UnitTests/Tests/Data"
)

$folders | ForEach-Object {
    New-Item -ItemType Directory -Path "$($variables.ProjectPrefix).$_" -Force
}

# Create solution items folder
New-Item -ItemType Directory -Path ".solution-items" -Force
Copy-Item $ConfigPath ".solution-items/scaffold.config.json"

# Generate README
@"
# $($variables.SolutionName)

## Overview

Generated using Scaffold CQRS template version $($config.templateVersion).

## Technology Stack

- .NET: $($variables.DotNetVersion)
- C#: $($variables.CSharpVersion)
- Database: $($variables.DatabaseProvider)
- ORM: Dapper v$($variables.DapperVersion)
- Authentication: $($variables.AuthMechanism)

## Getting Started

1. Update connection string in appsettings.json
2. Run database migrations
3. Build and run the solution

## Project Structure

- $($variables.ProjectPrefix).Common - Shared types
- $($variables.ProjectPrefix).Data - Data access layer
- $($variables.ProjectPrefix).Logic - Business logic
- $($variables.ProjectPrefix).API - API endpoints
- $($variables.ProjectPrefix).Integration - External services
- $($variables.ProjectPrefix).UnitTests - Test suite

## Documentation

For detailed documentation, see the docs/ folder.
"@ | Set-Content "README.md"

Write-Host "`nSolution generated successfully at: $solutionPath"
Write-Host "Next steps:"
Write-Host "1. Review the generated solution structure"
Write-Host "2. Update connection strings in appsettings.json"
Write-Host "3. Build the solution: dotnet build"
Write-Host "4. Run tests: dotnet test"
```

## Option 2: Using dotnet CLI Template (Coming Soon)

We're working on packaging this as a dotnet CLI template that can be installed and used like:

```powershell
# Install template (once)
dotnet new install Aspire.Scaffold.CQRS

# Create new solution
dotnet new aspire-cqrs --config scaffold.config.json
```

## Using the PowerShell Script

1. Save the script as `New-ScaffoldSolution.ps1`

2. Run it with your config file:
```powershell
.\New-ScaffoldSolution.ps1 -ConfigPath "scaffold.config.json"
```

The script will:
- Create solution and project structure
- Set up project references
- Install NuGet packages
- Create folder structure
- Generate basic README

## After Generation

1. Build the solution:
```powershell
dotnet build
```

2. Run tests:
```powershell
dotnet test
```

3. Update the connection string in `appsettings.json`

4. Start the API:
```powershell
dotnet run --project YourCompany.YourProject.API
```

## Customizing the Generated Solution

1. Update `appsettings.json` with your settings
2. Add domain entities in the Data layer
3. Create repositories and SQL queries
4. Implement CQRS commands and queries
5. Add API endpoints

## Common Issues

1. **Wrong .NET SDK Version**
   - Ensure you have .NET 9.0 SDK installed
   - Check global.json if present

2. **Package Version Conflicts**
   - Review package versions in scaffold.config.json
   - Update to compatible versions

3. **Path Too Long**
   - Use shorter solution/project names
   - Enable long paths in Windows

## Next Steps

1. Review the generated code
2. Add your domain entities
3. Implement business logic
4. Add API endpoints
5. Write unit tests

For more detailed instructions, refer to the scaffold documentation.