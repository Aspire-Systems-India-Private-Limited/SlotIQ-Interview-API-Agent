# Scaffold Instruction - Dapper CQRS 3.1

## Variable Definitions

All instructions use these standardized variables for consistent code generation:

| Variable | Description | Example Values |
|----------|-------------|----------------|
| `{{CompanyName}}` | Organization/Company name | Aspire, SlotIQ |
| `{{ProjectName}}` | Core project/product name | Interview, Inventory, CRM |
| `{{SolutionName}}` | Solution file name | SlotIQ.Interview |
| `{{ProjectPrefix}}` | Full namespace prefix | {{CompanyName}}.{{ProjectName}} |
| `{{DotNetVersion}}` | Target .NET version | net9.0, net8.0 |
| `{{CSharpVersion}}` | C# language version | 12.0, 11.0 |
| `{{DapperVersion}}` | Dapper ORM version | 2.1.35 |
| `{{ValidatorVersion}}` | FluentValidation version | 11.9.2 |
| `{{MapperVersion}}` | AutoMapper version | 12.0.1 |
| `{{TestingVersion}}` | xUnit version | 2.9.2 |
| `{{DatabaseProvider}}` | Database system | SqlServer, PostgreSQL |
| `{{AuthMechanism}}` | Authentication type | ActiveDirectory, JWT |

## General Instruction

This document defines the authoritative blueprint for generating enterprise-grade ASP.NET Core solutions using Clean Architecture, CQRS pattern with Dapper ORM, and Minimal APIs.

### Execution Requirements

- Every run must be treated as a clean, isolated generation
- Do not retain or reuse any artifacts from previous runs
- Generate the solution entirely from scratch using only this specification
- Follow naming conventions and architectural patterns exactly as outlined

## Solution Overview

Create a modern, maintainable backend service solution with:
- Clean Architecture principles
- Custom CQRS implementation (no MediatR)
- Dapper ORM with external SQL files
- Minimal APIs
- Comprehensive health checks
- Structured logging
- Full test coverage

### Required Capabilities

- Entity CRUD operations with validation
- Paged queries and filtering
- Global exception handling
- Health monitoring
- OpenAPI documentation
- Structured logging
- Transaction management
- Comprehensive test coverage

## Technology Stack

- **Language**: C# {{CSharpVersion}}
- **Framework**: ASP.NET Core {{DotNetVersion}} Web API
- **API Style**: Minimal APIs (static endpoint methods)
- **ORM**: Dapper v{{DapperVersion}}
- **Database**: {{DatabaseProvider}} with Microsoft.Data.SqlClient
- **Logging**: Serilog with console sink
- **Validation**: FluentValidation v{{ValidatorVersion}}
- **Mapping**: AutoMapper v{{MapperVersion}}
- **Documentation**: Swashbuckle.AspNetCore (Swagger/OpenAPI)
- **Testing**: xUnit v{{TestingVersion}}
- **Authentication**: {{AuthMechanism}}
- **JSON**: System.Text.Json with JsonStringEnumConverter
- **Health Checks**: Microsoft.Extensions.Diagnostics.HealthChecks with UI client

## Solution Structure

### Projects Layout

```
{{SolutionName}}/
├── {{SolutionName}}.sln
├── README.md
├── .runsettings                 # Test coverage configuration
├── global.json                  # SDK version pinning
│
├── {{ProjectPrefix}}.Common/     # Shared types & utilities
│   ├── Constants/              # Error messages, etc.
│   ├── Enums/                 # Domain enums
│   ├── Helpers/              # Extension methods
│   ├── Models/              # Result types
│   └── JsonConverters/    # Custom JSON converters
│
├── {{ProjectPrefix}}.Data/      # Data access layer  
│   ├── Entities/             # Domain entities
│   ├── Repositories/        # Data access implementations
│   │   └── Contracts/     # Repository interfaces
│   ├── Sql/               # SQL query files
│   ├── DbConnectionFactory.cs
│   ├── SqlQueryLoader.cs
│   └── UnitOfWork.cs
│
├── {{ProjectPrefix}}.Logic/     # Business logic layer
│   ├── Commands/             # Write operations
│   ├── Queries/            # Read operations
│   ├── Handlers/          # Command & query handlers
│   │   ├── Commands/    # Command handlers
│   │   └── Queries/    # Query handlers
│   ├── Dtos/           # Data transfer objects
│   ├── Mapping/       # AutoMapper profiles
│   └── Validators/   # FluentValidation validators
│
├── {{ProjectPrefix}}.API/       # API layer
│   ├── Configuration/        # Service configuration
│   ├── Endpoints/          # Minimal API endpoints
│   ├── HealthChecks/      # Health monitoring
│   ├── Middleware/       # Exception handling etc.
│   ├── Models/         # Request/response models
│   ├── Validators/    # Request validators
│   ├── Program.cs
│   └── appsettings.json
│
├── {{ProjectPrefix}}.Integration/  # External integrations
│   ├── Clients/               # Service clients
│   │   ├── Interfaces/      # Client interfaces
│   │   └── Implementations/ # Client implementations
│   ├── Configuration/       # Client configuration
│   └── Policies/          # Resilience policies
│
└── {{ProjectPrefix}}.UnitTests/   # Test project
    └── Tests/                   # Test classes
        ├── API/               # API tests
        ├── Logic/           # Handler tests
        └── Data/           # Repository tests
```

### Project Dependencies

| Project | References |
|---------|------------|
| Common  | None |
| Data    | Common |
| Logic   | Data, Integration, Common |
| API     | Logic, Data, Common |
| Integration | Common |
| UnitTests | All projects |

## Layer Implementation Details

### Common Layer ({{ProjectPrefix}}.Common)

#### Purpose
Shared types, constants, and utilities used across all layers.

#### Key Components

##### Result Pattern
```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    
    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string error) => new(error);
}
```

##### PaginatedResult Pattern
```csharp
public class PaginatedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }
}
```

### Data Layer ({{ProjectPrefix}}.Data)

#### Base Entity Pattern
```csharp
public abstract class BaseEntity
{
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string ModifiedBy { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
}
```

#### SQL File Structure
```
Sql/
├── {{EntityName}}/
│   ├── GetById.sql
│   ├── GetAll.sql
│   ├── GetPaged.sql
│   ├── Insert.sql
│   ├── Update.sql
│   └── Delete.sql
```

### Logic Layer ({{ProjectPrefix}}.Logic)

#### CQRS Interfaces
```csharp
public interface ICommand<TResult> { }
public interface IQuery<TResult> { }

public interface ICommandHandler<TCommand, TResult> 
    where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command);
}

public interface IQueryHandler<TQuery, TResult> 
    where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query);
}
```

### API Layer ({{ProjectPrefix}}.API)

#### Endpoint Registration Pattern
```csharp
public static class {{EntityName}}Endpoints
{
    public static IEndpointRouteBuilder Map{{EntityName}}Endpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/{{entityNamePlural}}")
            .WithTags("{{EntityName}}")
            .WithOpenApi();

        group.MapGet("/", GetAll{{EntityNamePlural}})
            .WithName("GetAll{{EntityNamePlural}}")
            .Produces<ApiResponse<IEnumerable<{{EntityName}}Dto>>>()
            .ProducesValidationProblem();

        // Additional endpoints...
        
        return app;
    }
}
```

## Required NuGet Packages

### Common Layer
```xml
<ItemGroup>
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="{{DotNetVersion}}" />
    <PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="{{DotNetVersion}}" />
</ItemGroup>
```

### Data Layer
```xml
<ItemGroup>
    <PackageReference Include="Dapper" Version="{{DapperVersion}}" />
    <PackageReference Include="Microsoft.Data.SqlClient" Version="5.1.5" />
</ItemGroup>
```

### Logic Layer
```xml
<ItemGroup>
    <PackageReference Include="AutoMapper" Version="{{MapperVersion}}" />
    <PackageReference Include="FluentValidation" Version="{{ValidatorVersion}}" />
</ItemGroup>
```

### API Layer
```xml
<ItemGroup>
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
    <PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />
</ItemGroup>
```

## Configuration Templates

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server={{DatabaseServer}};Database={{DatabaseName}};Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    }
  },
  "HealthChecks": {
    "SqlServer": {
      "Timeout": "00:00:30"
    }
  }
}
```

## Implementation Checklist

1. **Initial Setup**
   - [ ] Replace all placeholder variables
   - [ ] Create solution structure
   - [ ] Configure project references
   - [ ] Install NuGet packages

2. **Common Layer**
   - [ ] Implement Result patterns
   - [ ] Add common enums
   - [ ] Create error messages

3. **Data Layer**
   - [ ] Set up base entity
   - [ ] Implement repositories
   - [ ] Create SQL files

4. **Logic Layer**
   - [ ] Set up CQRS interfaces
   - [ ] Implement handlers
   - [ ] Configure AutoMapper

5. **API Layer**
   - [ ] Configure startup
   - [ ] Implement endpoints
   - [ ] Set up health checks

6. **Testing**
   - [ ] Write unit tests
   - [ ] Configure coverage
   - [ ] Validate patterns

## Quality Standards

1. **Code Coverage**
   - Minimum 90% coverage required
   - Include all layers except Program.cs

2. **Performance**
   - Async operations throughout
   - Proper connection management
   - Query optimization

3. **Security**
   - Input validation
   - Proper authentication
   - SQL injection prevention

4. **Maintainability**
   - Clean architecture
   - SOLID principles
   - Comprehensive documentation

## Support

For questions or issues:
- Create an issue in the repository
- Contact the architecture team
- Reference the example implementation at [{{CompanyName}} Reference Architecture Repository]