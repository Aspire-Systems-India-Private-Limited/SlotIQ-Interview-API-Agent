# Scaffold Instruction - Dapper CQRS 3.0

## General Instruction

This document defines the authoritative blueprint for generating enterprise-grade ASP.NET Core solutions using Clean Architecture, CQRS pattern with Dapper ORM, and Minimal APIs. It reflects the latest .NET 9 patterns and best practices.

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

- **Language**: C# 12
- **Framework**: ASP.NET Core 9.0 Web API
- **API Style**: Minimal APIs (static endpoint methods)
- **ORM**: Dapper v2.1.35
- **Database**: SQL Server with Microsoft.Data.SqlClient
- **Logging**: Serilog with console sink
- **Validation**: FluentValidation v11.9.2
- **Mapping**: AutoMapper v12.0.1
- **Documentation**: Swashbuckle.AspNetCore (Swagger/OpenAPI)
- **Testing**: xUnit v2.9.2 with Moq v4.20.72
- **Assertions**: FluentAssertions v6.12.1
- **JSON**: System.Text.Json with JsonStringEnumConverter
- **Health Checks**: Microsoft.Extensions.Diagnostics.HealthChecks with UI client
- **Coverage**: coverlet.collector and coverlet.msbuild

## Solution Structure

### Projects Layout

```
SlotIQ.Interview/
├── SlotIQ.sln
├── README.md
├── .runsettings                 # Test coverage configuration
├── global.json                  # SDK version pinning
│
├── SlotIQ.Interview.Common/     # Shared types & utilities
│   ├── Constants/              # Error messages, etc.
│   ├── Enums/                 # Domain enums
│   ├── Helpers/              # Extension methods
│   ├── Models/              # Result types
│   └── JsonConverters/    # Custom JSON converters
│
├── SlotIQ.Interview.Data/      # Data access layer  
│   ├── Entities/             # Domain entities
│   ├── Repositories/        # Data access implementations
│   │   └── Contracts/     # Repository interfaces
│   ├── Sql/               # SQL query files
│   ├── DbConnectionFactory.cs
│   ├── SqlQueryLoader.cs
│   └── UnitOfWork.cs
│
├── SlotIQ.Interview.Logic/     # Business logic layer
│   ├── Commands/             # Write operations
│   ├── Queries/            # Read operations
│   ├── Handlers/          # Command & query handlers
│   │   ├── Commands/    # Command handlers
│   │   └── Queries/    # Query handlers
│   ├── Dtos/           # Data transfer objects
│   ├── Mapping/       # AutoMapper profiles
│   └── Validators/   # FluentValidation validators
│
├── SlotIQ.Interview.API/       # API layer
│   ├── Configuration/        # Service configuration
│   ├── Endpoints/          # Minimal API endpoints
│   ├── HealthChecks/      # Health monitoring
│   ├── Middleware/       # Exception handling etc.
│   ├── Models/         # Request/response models
│   ├── Validators/    # Request validators
│   ├── Program.cs
│   └── appsettings.json
│
├── SlotIQ.Interview.Integration/  # External integrations
│   ├── Clients/               # Service clients
│   │   ├── Interfaces/      # Client interfaces
│   │   └── Implementations/ # Client implementations
│   ├── Configuration/       # Client configuration
│   └── Policies/          # Resilience policies
│
└── SlotIQ.Interview.UnitTests/   # Test project
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

### Common Layer (SlotIQ.Interview.Common)

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

##### ApiResponse Pattern
```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
}
```

### Data Layer (SlotIQ.Interview.Data)

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

#### Repository Pattern
```csharp
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<Result<TEntity>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<TEntity>>> GetAllAsync();
    Task<PaginatedResult<TEntity>> GetPagedAsync(int pageNumber, int pageSize);
    Task<Result<TEntity>> AddAsync(TEntity entity);
    Task<Result<TEntity>> UpdateAsync(TEntity entity);
    Task<Result<bool>> DeleteAsync(Guid id);
}
```

#### SQL File Structure
```
Sql/
├── {EntityName}/
│   ├── GetById.sql
│   ├── GetAll.sql
│   ├── GetPaged.sql
│   ├── Insert.sql
│   ├── Update.sql
│   └── Delete.sql
```

### Logic Layer (SlotIQ.Interview.Logic)

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

#### Command/Query Examples
```csharp
// Commands
public record CreateEntityCommand(CreateEntityDto Dto) 
    : ICommand<Result<EntityDto>>;

// Queries
public record GetEntityByIdQuery(Guid Id) 
    : IQuery<Result<EntityDto>>;
```

#### Handler Examples
```csharp
public class CreateEntityHandler 
    : ICommandHandler<CreateEntityCommand, Result<EntityDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public async Task<Result<EntityDto>> HandleAsync(
        CreateEntityCommand command)
    {
        // Implementation with transaction management
    }
}
```

### API Layer (SlotIQ.Interview.API)

#### Endpoint Registration Pattern
```csharp
public static class EntityEndpoints
{
    public static IEndpointRouteBuilder MapEntityEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/entities")
            .WithTags("Entities")
            .WithOpenApi();

        group.MapGet("/", GetAllEntities)
            .WithName("GetAllEntities")
            .Produces<ApiResponse<IEnumerable<EntityDto>>>()
            .ProducesValidationProblem();

        // Additional endpoints...
        
        return app;
    }

    private static async Task<IResult> GetAllEntities(
        IQueryHandler<GetAllEntitiesQuery, Result<IEnumerable<EntityDto>>> handler)
    {
        var result = await handler.HandleAsync(new GetAllEntitiesQuery());
        return result.IsSuccess
            ? TypedResults.Ok(new ApiResponse<IEnumerable<EntityDto>> 
                { Success = true, Data = result.Value })
            : TypedResults.BadRequest(new ApiResponse<IEnumerable<EntityDto>> 
                { Success = false, ErrorMessage = result.Error });
    }
}
```

#### Health Check Pattern
```csharp
public class LivenessHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        // Basic application status check
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}

public class ReadinessHealthCheck : IHealthCheck 
{
    private readonly IDbConnectionFactory _db;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Check database connectivity
            using var conn = _db.CreateConnection();
            await conn.OpenAsync(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(ex.Message);
        }
    }
}
```

### Unit Testing Layer (SlotIQ.Interview.UnitTests)

#### Test Organization
```
Tests/
├── API/
│   └── Endpoints/
│       └── EntityEndpointsTests.cs
├── Logic/
│   ├── Commands/
│   │   └── CreateEntityHandlerTests.cs
│   └── Queries/
│       └── GetEntityByIdHandlerTests.cs
└── Data/
    └── Repositories/
        └── EntityRepositoryTests.cs
```

#### Test Pattern Example
```csharp
public class CreateEntityHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IMapper> _mapper;
    private readonly CreateEntityHandler _handler;

    public CreateEntityHandlerTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _mapper = new Mock<IMapper>();
        _handler = new CreateEntityHandler(_unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var command = new CreateEntityCommand(new CreateEntityDto());
        
        // Act
        var result = await _handler.HandleAsync(command);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        _unitOfWork.Verify(x => x.CommitAsync(), Times.Once);
    }
}
```

## Required NuGet Packages

### Common Layer
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Logging.Abstractions

### Data Layer
- Dapper
- Microsoft.Data.SqlClient
- Microsoft.Extensions.Configuration.Abstractions

### Logic Layer
- AutoMapper
- AutoMapper.Extensions.Microsoft.DependencyInjection
- FluentValidation
- FluentValidation.DependencyInjectionExtensions

### API Layer
- Microsoft.AspNetCore.OpenApi
- Swashbuckle.AspNetCore
- Serilog.AspNetCore
- Serilog.Sinks.Console
- Serilog.Enrichers.Environment
- Microsoft.Extensions.Diagnostics.HealthChecks
- AspNetCore.HealthChecks.SqlServer
- AspNetCore.HealthChecks.UI.Client

### Integration Layer
- Microsoft.Extensions.Http
- Microsoft.Extensions.Http.Polly
- Polly
- Polly.Extensions.Http

### Unit Testing Layer
- xUnit
- xUnit.runner.visualstudio
- Moq
- FluentAssertions
- coverlet.collector
- coverlet.msbuild

## Prohibited Patterns

1. DO NOT use:
   - Entity Framework Core (use Dapper)
   - MediatR (use custom CQRS)
   - Controllers (use Minimal APIs)
   - Dynamic SQL (use SQL files)
   - Hard deletes (use soft delete pattern)

2. ALWAYS:
   - Use external SQL files
   - Implement proper error handling
   - Return Result<T> from operations
   - Use structured logging
   - Include unit tests
   - Follow naming conventions

## Required Configuration Files

### Solution Root
- `.runsettings` for test coverage configuration
- `global.json` for SDK version pinning
- `README.md` with setup instructions

### API Project
- `appsettings.json` with:
  - Connection strings
  - Logging settings
  - Health check configuration
- `appsettings.Development.json`
- `Properties/launchSettings.json`

## Implementation Order

1. Create solution and project structure
2. Configure project references and NuGet packages
3. Implement Common layer base types
4. Set up Data layer with base repository
5. Implement Logic layer CQRS components
6. Create API endpoints with health checks
7. Add Integration layer structure
8. Write unit tests
9. Configure CI/CD pipeline

## Critical Success Metrics

- All projects build successfully
- Unit tests pass with >90% coverage
- No compiler warnings
- Clean architecture principles maintained
- Proper separation of concerns
- Comprehensive documentation
- Health checks implemented and working
- Structured logging in place
- OpenAPI documentation generated