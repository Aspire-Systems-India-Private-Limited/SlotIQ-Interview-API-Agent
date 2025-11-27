# PowerShell script to generate solution from scaffold configuration
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