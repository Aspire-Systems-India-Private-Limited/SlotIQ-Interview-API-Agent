## General Instruction
This document defines the authoritative blueprint for generating a fully isolated, enterprise-grade, multi-project solution using Dapper ORM with CQRS pattern, Unit of Work, and dedicated SQL query files.

### Execution Requirements
- Every run must be treated as a clean, isolated generation.
- Clear all memory of previously generated content before starting.
- Do not retain or reuse any artifacts from previous runs, including:
  - Entities, DTOs, interfaces, mappings, files, or project references.
- Generate the solution entirely from scratch, based only on the definitions in this file.
- Do not assume defaults or infer unspecified behavior.
  - If something is not explicitly defined here, do not generate it.
- Follow all structure, naming conventions, and technology selections exactly as outlined.

## Solution Overview
Scaffold a modular, backend service solution using ASP.NET Core with Clean Architecture principles, CQRS pattern, and a multi-project structure. The generated codebase must support maintainability, testability, and extensibility for enterprise-scale development with clear separation between Commands and Queries.

Each solution must:
- Target {DotNet Version}
- Use a layered architecture with clearly separated concerns
- Implement CQRS pattern with dedicated Command and Query handlers
- Apply Unit of Work pattern for transaction management
- Use dedicated SQL query files for all database operations
- Apply dependency injection throughout all layers
- Implement standardized API design using the specified {API Type}
- Secure endpoints using {Authentication Mechanism}
- Persist data using the configured {ORM} with raw SQL queries
- Apply consistent mapping, logging, serialization, and documentation practices
- Support comprehensive unit testing using {Testing Framework} and {Mocking Framework}
- Add required using statements for all layers classes
- Add launchsettings.json file in {API Layer}
All libraries, packages, and configurations must align with the technology stack and project layout defined below.

## Tech Stack & Project Specifications
- Language: {Language} = C#
- Framework: {Framework} = ASP.NET Core Web API
- .NET Version: {DotNet Version} = .NET 9
- API Type: {API Type} = Minimal APIs
- ORM: {ORM} = Dapper (SQL Server)
- Database Provider: {Database Provider} = Microsoft.Data.SqlClient
- Authentication Mechanism: {Authentication Mechanism} = Active Directory Integration
- Logging Framework: {Logging Framework} = Serilog (Serilog.AspNetCore, Serilog.Sinks.Console)
- Mapping Library: {Mapping Library} = AutoMapper
- Serialization: {Serialization} = System.Text.Json (JsonStringEnumConverter)
- API Style: {API Style} = Minimal APIs
- API Documentation Tool: {Documentation} = Swashbuckle (Swagger)
- Testing Framework: {Testing Framework} = xUnit
- Mocking Framework: {Mocking Framework} = Moq
- Project Name: {Project Name} = SlotIQ.Interview

## Project Structure

### Solution Definition
- Solution Name: {Solution Name} = SlotIQ
Create a root-level `.sln` file with the name defined in `{Solution Name}`. This solution must include all projects listed below and correctly configure project-to-project references.

### Project Layers and Naming
Each layer must be implemented as a separate project with its own physical folder and `.csproj` file. Use the following naming conventions:

- Shared/Common Layer: {Shared Layer} = {Project Name}.Common
- Data Access Layer: {Data Access Layer} = {Project Name}.Data
- Business Logic Layer: {Business Logic Layer} = {Project Name}.Logic
- API Layer: {API Layer} = {Project Name}.API
- Integration/Client Layer: {Integration Layer} = {Project Name}.Integration
- Unit Testing Layer: {Unit Testing Layer} = {Project Name}.UnitTests

### Project Reference Rules
| Project                | Reference                                  				    |
|------------------------|--------------------------------------------------------------|
| {Common Layer}         | (None – must remain independent)               				|
| {Data Access Layer}    | {Shared Layer} only                            				|
| {Business Logic Layer} | {Data Access Layer}, {Integration Layer}, , {Shared Layer}	|
| {API Layer}            | {Business Logic Layer}, {Data Access Layer}, {Shared Layer}	|
| {Integration Layer}    | {Shared Layer}	                                            |
| {Unit Testing Layer}   | All other layers                               				|

### Project File (`.csproj`) Requirements
For each project, the `.csproj` file must include:
- The correct project name
- Target framework set to `{DotNet Version}`
- Nullable reference types enabled
- Implicit usings disabled
- All required NuGet package references, based on the tech stack:
  - e.g., `{ORM}`, `{Database Provider}`, `{Logging Framework}`, `{Mapping Library}`, `{Testing Framework}`, `{Mocking Framework}`
- Required project references based on architectural rules above

### Solution File (`.sln`) Requirements
- Create the solution at the root of the workspace.
- Name it according to `{Solution Name}`.
- Include all project layers:
  - {Shared Layer}
  - {Data Access Layer}
  - {Business Logic Layer}
  - {API Layer}
  - {Integration Layer}
  - {Unit Testing Layer}
- Ensure that all inter-project dependencies are configured in the solution file and resolve correctly during build.

### Code Coverage Configuration
- Create a `.runsettings` file at the solution root with the following specifications:
  - Target minimum code coverage: 90% line coverage
  - Exclude auto-generated files and startup configuration
  - Include coverage for layers {Shared Layer}, {Data Access Layer}, {Business Logic Layer}, {API Layer} and {Integration Layer}
  - Define threshold rules for build/PR validation

## Supported Entities
Each layer must provide full support for all entities defined in the `{Entity Definitions}` section at the end of this file. Do not hardcode, rename, or skip any entities unless explicitly instructed.

### Logging
- Use the logging framework specified by `{Logging Framework}`
- Integrate structured logging using a framework-specific sink (e.g., console, file, Seq)
- Enrich logs with metadata such as:
  - Correlation ID
  - User or caller identity
  - HTTP route or operation context
  - Command/Query type and execution time
- Implement global exception handling middleware to capture and log unhandled exceptions consistently

### Dependency Injection
- Register services, repositories, database contexts, command/query handlers, and mapping profiles using the built-in DI container
- Use constructor injection consistently across all layers
- Group service registrations into modular static extension methods (e.g., `ServiceCollectionExtensions`)
- Register all command and query handlers automatically using reflection or manual registration
- Keep `Program.cs` minimal by offloading configuration and service registration to dedicated classes

### Serialization
- Use the serialization mechanism defined in `{Serialization}`
- Configure global serialization options for all endpoints
- Register `JsonStringEnumConverter` to ensure enum values are serialized as strings
- Avoid hardcoded serialization logic inside endpoints or handlers

### API Layer Design Rules
- The API layer must not contain business logic
- It should act purely as a transport boundary between HTTP and command/query handlers
- Every route must delegate directly to the appropriate command or query handler
- Parameter names, shapes, and return types must match the handler contract exactly
- All route patterns must be predictable and consistent across entities

### API Documentation
- Use the documentation tool specified in `{Documentation}`
- Enable documentation in non-production environments only
- Configure required services:
  - `AddEndpointsApiExplorer`
  - `{Documentation}` package (e.g., Swashbuckle) to generate OpenAPI metadata
- For each endpoint:
  - Apply `.WithSummary(...)` and `.WithDescription(...)`
  - Document input models with `.Accepts<T>("application/json")`
  - Document return types with `.Produces<T>(...)` for each HTTP response (200, 201, 400, 404)
  - Assign route names using `.WithName(...)` for client code generation and metadata lookup
- Ensure all endpoints are documented without exception

## Responsibilities by Layer

### {Shared Layer}
This project defines all cross-cutting types, constants, enums, models, and helpers that are shared across the solution.

Rules:
- This project must not reference or depend on any other layer
- All types in this layer must be self-contained and framework-agnostic

#### Constants
- Define all global constants in a `Constants/` folder
- Include configuration keys, error messages, and validation strings
- Group constants by logical purpose or domain
- Example file: `Constants/ErrorMessages.cs`

#### Enums
- Place all enums defined in `{Enum Definitions}` inside the `Enums/` folder
- Declare each enum as: `public enum {Name}`
- Enum member names must follow PascalCase
- All enums must serialize as strings in JSON using:
  - `[JsonConverter(typeof(JsonStringEnumConverter))]`
- Do not assign explicit numeric values unless required for interoperability or external mapping

#### Shared Models
- Place all reusable wrapper types in a `Models/` folder

Required types:
- `Result<T>`:
  - A generic wrapper representing success/failure outcomes
  - Properties: `bool IsSuccess`, `T? Value`, `string? Error`
  - Must include static factory methods:
    - `Result<T>.Success(T value)`
    - `Result<T>.Failure(string error)`
  - Handle nullability warnings using correct annotations or constructor defaults

- `ApiResponse<T>`:
  - A simplified wrapper for HTTP responses
  - Properties: `bool Success`, `T? Data`, `string? ErrorMessage`

- `PaginatedResult<T>`:
  - A wrapper for paginated collections
  - Properties:
    - `IEnumerable<T> Items`
    - `int PageNumber`
    - `int PageSize`
    - `int TotalCount`
    - `bool IsSuccess`
    - `string Error`

#### Helpers
- Place utility classes and converters in a `Helpers/` folder
- Include a `ResultExtensions` static class with at least:
  - Extension methods for fluent chaining or conversions from `Result<T>`
- Register `JsonStringEnumConverter` globally to support string-based enum serialization

### {Data Access Layer}

This project defines persistence models, database connections, repository implementations, and SQL query management using Dapper.
The `{Data Access Layer}` must only reference the `{Shared Layer}`.

#### SQL Query File Structure
- Create `Sql/` folder in {Data Access Layer}

#### Entities
- Create `Entities/` folder in {Data Access Layer}
- Create `BaseEntity.cs` in `Entities/` folder and add properties like `Id`, `CreatedAt`, `CreatedBy`, etc.

#### Database Connection
- Implement `IDbConnectionFactory` for creating database connections
- Use connection string from configuration
- Support both read and write connections if needed
- Implement proper connection disposal and error handling

```csharp
public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
    Task<IDbConnection> CreateConnectionAsync();
}
```

#### SQL Query Management
- Implement `ISqlQueryLoader` to load SQL queries from `.sql` files
- Embed SQL files as resources or read from file system
- Cache loaded queries for performance
- Support parameterized query loading

#### Repositories
Repository interfaces:
- Define one interface per entity in a `Repositories/Contracts/` folder
- Include all required CRUD and pagination methods
- Related or nested entities (e.g., child collections) must also have their own repositories

Repository implementations:
- Place in a corresponding `Repositories/` folder
- Implement each interface completely using Dapper best practices
- Use `async`/`await` and return types from the `{Shared Layer}`
- Load SQL queries using `ISqlQueryLoader`
- Use parameterized queries for all database operations

Repository methods must follow exact naming and signature conventions:
```csharp
Task<Result<TEntity>> GetByIdAsync(Guid id);
Task<Result<IEnumerable<TEntity>>> GetAllAsync();
Task<PaginatedResult<TEntity>> GetPagedAsync(int pageNumber, int pageSize);
Task<Result<TEntity>> AddAsync(TEntity entity);
Task<Result<TEntity>> UpdateAsync(TEntity entity);
Task<Result<bool>> DeleteAsync(Guid id);
```

#### Unit of Work Implementation
- Implement `IUnitOfWork` interface with transaction management
- Manage `IDbConnection` and `IDbTransaction` lifecycle
- Provide access to all repositories through the Unit of Work
- Implement proper disposal pattern

### {Business Logic Layer}
This project contains all core application logic using CQRS pattern with command and query handlers. It must remain independent of frameworks and infrastructure concerns, and only reference the `{Data Access Layer}` and `{Shared Layer}`.

#### DTOs
- Create dedicated DTOs for each entity to represent both input and output data structures
- Do not reuse domain models in API contracts
- Place DTO classes in a dedicated `Dtos/` folder within the `{Business Logic Layer}`

Use the following consistent naming conventions:
- `{Entity}Dto` — for read/output responses
- `Create{Entity}Dto` — for POST requests
- `Update{Entity}Dto` — for PUT requests

#### Commands and Queries
- Separate all operations into Commands (write) and Queries (read)
- Place commands in `Commands/` folder and queries in `Queries/` folder
- Each command/query must be immutable and contain all necessary data

#### Command Structure
```csharp
public record Create{Entity}Command(Create{Entity}Dto Dto) : ICommand<Result<{Entity}Dto>>;
public record Update{Entity}Command(Guid Id, Update{Entity}Dto Dto) : ICommand<Result<{Entity}Dto>>;
public record Delete{Entity}Command(Guid Id) : ICommand<Result<bool>>;
```

#### Query Structure
```csharp
public record Get{Entity}ByIdQuery(Guid Id) : IQuery<Result<{Entity}Dto>>;
public record GetAll{Entity}Query() : IQuery<Result<IEnumerable<{Entity}Dto>>>;
public record GetPaged{Entity}Query(int PageNumber, int PageSize) : IQuery<PaginatedResult<{Entity}Dto>>;
```

#### Command Handlers
For each entity in `{Entity Definitions}`:
- Create command handlers in `Handlers/Commands/` folder
- Use constructor injection for `IUnitOfWork` and `IMapper`
- Implement transaction management using Unit of Work
- Map DTOs to domain models before repository operations

#### Query Handlers
For each entity in `{Entity Definitions}`:
- Create query handlers in `Handlers/Queries/` folder
- Use constructor injection for repositories and `IMapper`
- Use direct repository access without transactions for read operations
- Map repository results to DTOs before returning

#### Mapping
- Use `{Mapping Library}` (e.g., AutoMapper) for all object-to-object transformations
- Define one AutoMapper profile per entity in a `Mapping/` or `Profiles/` folder
- Register all profiles at application startup
- Mapping rules:
  - Incoming DTOs must be mapped to domain models before repository calls
  - Repository results must be mapped to DTOs before returning to the API layer
- Do not pass DTOs into repository methods, convert to domain models before use

#### Error Handling
- Do not hardcode error messages in handler classes
- Reference all user-facing messages from `Constants/ErrorMessages.cs` in the `{Shared Layer}`
- If an error message is missing, define a new constant
- Use fully qualified names when necessary to avoid ambiguity

#### Handler Registration
- Register all command and query handlers in DI container
- Use assembly scanning or manual registration
- Ensure proper lifetime management (typically Scoped)

### {API Layer}
This project exposes public endpoints and wires up the application runtime using the approach specified in `{API Style}`. It must only reference the `{Business Logic Layer}`, `{Data Access Layer}`, and `{Shared Layer}`.

#### Endpoint Registration

If `{API Style}` = Minimal APIs:
- Use route groups and static extension methods to register endpoints per entity
- Place all endpoint registration classes inside an `Endpoints/` folder
- Group endpoints using `.MapGroup("/[entity]")`
- Define one static method per entity that registers all CRUD operations
- Each route must call the corresponding command or query handler:
  - Query handlers for GET operations
  - Command handlers for POST, PUT, DELETE operations
- Return types must use `Result<T>` or `PaginatedResult<T>` and be translated to appropriate HTTP responses
- Register all endpoint groups in `Program.cs` using extension methods (e.g., `app.Map{EntityName}Endpoints()`)

#### Handler Injection
- Inject command and query handlers into endpoints
- Use mediator pattern if preferred (e.g., MediatR)
- Ensure proper error handling and response mapping

#### API Documentation
- Follow the API documentation standards defined in the `API Documentation` section under `Cross-Cutting Design Patterns`.

#### Startup Responsibilities

##### Middleware Configuration
- Use `UseHttpsRedirection()`
- Use centralized exception handling middleware with structured logging using `{Logging Framework}`
- Apply the authentication strategy defined in `{Authentication Mechanism}`

If `{Authentication Mechanism}` = Active Directory Integration:
- Use `System.DirectoryServices.AccountManagement`
- Implement authentication using:
  - `PrincipalContext`
  - `UserPrincipal.FindByIdentity`
- Extract the username from a configurable HTTP header (e.g., `X-Username`)
- Bind Active Directory settings from `appsettings.json` using `IOptions<ActiveDirectorySettings>`
- Return `401 Unauthorized` on failure
- Log all authentication attempts
- Encapsulate logic in an extension method: `UseActiveDirectoryAuthentication()`

#### Dependency Injection
- Follow the service registration rules defined in the `Dependency Injection` section under `Cross-Cutting Design Patterns`.
- Register all command and query handlers
- Register Unit of Work and repositories
- Register SQL query loader and database connection factory

#### Health Checks
- Implement health checks using build in Microsoft.Extensions.Diagnostics.HealthChecks
- Create multiple health check types:
  - `LivenessCheck`: Basic application availability
  - `ReadinessCheck`: Service dependencies and SQL Server connection are available and ready
- Configure health check endpoints:
  - GET /health/live - Quick check if service is running. This should validate only application stablibility and should not include any external dependencies.
  - GET /health/ready - Comprehensive check of all dependencies. This should validate all external dependencies and application stability.
- Add AspNetCore.HealthChecks.UI.Client for formatted responses
- Register all health checks in Program.cs using extension methods
- Include health check status in logging and monitoring
- If there is any exception log it, dont show exception details in response.

### {Integration Layer}
This project handles all external API calls and integrations with third-party services.

#### Client Interfaces
- Define interfaces for all external service clients in `Clients/Interfaces/`
- Each interface should follow `I{ServiceName}Client` naming convention
- Methods should return `Task<Result<T>>` to maintain consistency with domain results

#### Client Implementations
- Place implementations in `Clients/Implementations/`
- Use HttpClientFactory for proper lifecycle management
- Implement retry policies and circuit breakers using Polly
- Encapsulate all serialization/deserialization logic
- Handle authentication and authorization for external services

#### Resilience Policies
- Define resilience policies (retry, circuit breaker, timeout) in a dedicated folder
- Implement policy registry for centralized configuration
- Configure different policies based on service characteristics

#### Configuration
- Create POCO classes for client configuration in `Configuration/`
- Use IOptions pattern for dependency injection
- Document all configuration properties thoroughly


### {Unit Testing Layer}
This project contains all unit tests and must reference the `{API Layer}`, `{Business Logic Layer}`, `{Data Access Layer}`, and `{Shared Layer}`.

#### Scope
For each entity in `{Entity Definitions}`:
- Write unit tests for command handlers in the `{Business Logic Layer}`
- Write unit tests for query handlers in the `{Business Logic Layer}`
- Write unit tests for repository classes in the `{Data Access Layer}`
- Write unit tests for API endpoints by mocking handler dependencies

#### Rules
- Use `{Testing Framework}` for test organization and execution
- Use `{Mocking Framework}` to isolate dependencies and assert behavior
- Mock `IUnitOfWork`, repositories, and handlers appropriately
- Do not include integration tests, database calls, or UI tests
- Structure tests to validate:
  - Expected return values and error states
  - Input validation paths
  - Dependency interaction (e.g., `Verify` handler calls)
  - Transaction management in command handlers
- Keep tests atomic and layer-isolated
- Place all test classes in a `Tests/` folder structure mirroring the project under test
- Prefer fluent assertions or expressive assertion libraries for clarity (e.g., FluentAssertions)

#### Project Setup
- Target the same `{DotNet Version}` as the main projects
- Reference all projects under test (e.g., `SlotIQ.Interview.Logic`, `SlotIQ.Interview.Data`, `SlotIQ.Interview.API`)
- Register `{Testing Framework}` and `{Mocking Framework}` as NuGet dependencies

## NuGet Package References
- Install latest stable version of below NuGet packages
### Core Packages (All Projects)
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Logging.Abstractions

### {API Layer} Packages
- Microsoft.AspNetCore.OpenApi
- Swashbuckle.AspNetCore
- Serilog.AspNetCore
- Serilog.Sinks.Console
- Serilog.Enrichers.Environment
- Microsoft.Extensions.Diagnostics.HealthChecks
- AspNetCore.HealthChecks.SqlServer
- AspNetCore.HealthChecks.UI.Client

### {Business Logic Layer} Packages
- AutoMapper
- AutoMapper.Extensions.Microsoft.DependencyInjection
- FluentValidation
- FluentValidation.DependencyInjectionExtensions

### {Data Access Layer} Packages
- Dapper
- Microsoft.Data.SqlClient
- Microsoft.Extensions.Configuration.Abstractions

### {Integration Layer} Packages
- Microsoft.Extensions.Http
- Microsoft.Extensions.Http.Polly
- Polly
- Polly.Extensions.Http

### {Unit Testing Layer} Packages
- xUnit
- xUnit.runner.visualstudio
- Moq
- FluentAssertions
- coverlet.collector
- coverlet.msbuild
