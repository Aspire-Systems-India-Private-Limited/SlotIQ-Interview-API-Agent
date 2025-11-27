---
applyTo: SlotIQ.Interview.API/**/*.cs
description: API layer patterns for Minimal APIs, endpoints, middleware, health checks, and authentication using ASP.NET Core 9
---

## Purpose
Defines Minimal API endpoints, middleware, health checks, and authentication. The API layer accepts HTTP requests, validates and documents them, and delegates to CQRS handlers in the Logic layer. It returns consistent JSON responses using `ApiResponse<T>` and `TypedResults`.

## Variables
- `{{EntityName}}`: singular entity (e.g., AvailabilityPlan)
- `{{EntityNamePlural}}`: plural entity (e.g., AvailabilityPlans)
- `{{ProjectPrefix}}`: root namespace (e.g., SlotIQ.Interview)

## Core Principles
- Minimal APIs only. No controllers.
- Static endpoint methods with direct handler injection.
- Consistent `ApiResponse<T>` wrapper with `TypedResults`.
- OpenAPI metadata for every endpoint.
- Global exception handling middleware.
- Health checks for liveness and readiness.
- File-scoped namespaces in all C# files.
- Primary keys use `{{EntityName}}ID` (ID suffix, not Id).
- Add Swagger UI, install its dependent packages and register it.
- Follow contract SlotIQ.yml, for defining endpoint path, request and response objects.
- AutoMapper for Request-DTO mapping.

## File Organization
- Place all endpoint classes in the `Endpoints` folder
- Group related endpoints in a single file
- Place middleware in the `Middleware` folder
- Place health checks in the `HealthChecks` folder
- Place authentication/authorization in the `Authentication` folder
- Place service configuration extensions in the `Configuration` folder

## Endpoint Structure
- File: `SlotIQ.Interview.API/Endpoints/{{EntityName}}Endpoints.cs`
- Class: `{{EntityName}}Endpoints`
- Mapper: `Map{{EntityName}}Endpoints(this IEndpointRouteBuilder app)`
- Group: `/{{entityNamePlural}}` (lowercase plural) with `.WithTags("{{EntityNamePlural}}").WithOpenApi()`
- Document endpoints with `.WithName()`, `.WithSummary()`, `.WithDescription()`, `.Accepts<T>()`, `.Produces<T>()`
- Provide GET by ID, POST Create, PUT Update, DELETE, GET Paged as needed

## Endpoint Method Patterns
- Inject the relevant handler directly as a parameter.
- Use the appropriate command/query record type.
- Translate `Result<T>` into `TypedResults` and `ApiResponse<T>`.
- Detect not-found using error message content contains "not found" (case-insensitive), otherwise 400 for business errors, 500 for unexpected errors.

Canonical examples (copy and adapt for all endpoints):

GET by ID (404 via error message, else 500):
```csharp
private static async Task<Results<Ok<ApiResponse<{{EntityName}}Dto>>, NotFound<ApiResponse<object>>, StatusCodeHttpResult>> Get{{EntityName}}ById(
    Guid id,
    Get{{EntityName}}ByIdQueryHandler handler,
    CancellationToken ct)
{
    var result = await handler.Handle(new Get{{EntityName}}ByIdQuery(id), ct);
    if (!result.IsSuccess)
        return result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true
            ? TypedResults.NotFound(new ApiResponse<object> { Success = false, ErrorMessage = result.Error })
            : TypedResults.StatusCode(500);
    return TypedResults.Ok(new ApiResponse<{{EntityName}}Dto> { Success = true, Data = result.Value });
}
```

POST Create (400 for business/validation errors):
```csharp
private static async Task<Results<Created<ApiResponse<{{EntityName}}Dto>>, BadRequest<ApiResponse<object>>>> Create{{EntityName}}(
    Create{{EntityName}}Request request,
    Create{{EntityName}}CommandHandler handler,
    CancellationToken ct)
{
    var create{{EntityName}}Dto = mapper.Map<Create{{EntityName}}Dto>(request);
    var result = await handler.Handle(new Create{{EntityName}}Command(dto), ct);
    if (!result.IsSuccess)
        return TypedResults.BadRequest(new ApiResponse<object> { Success = false, ErrorMessage = result.Error });
    return TypedResults.Created($"/{{entityNamePlural}}/{result.Value!.{{EntityName}}ID}", new ApiResponse<{{EntityName}}Dto> { Success = true, Data = result.Value });
}
```

Other patterns (apply same result mapping):
- Update: return 200 on success; 404 when error contains "not found"; else 400.
- Delete: return 204 on success; 404 when "not found"; else 500.
- GetPaged: return 200 with `PaginatedResult<T>`; 400 for errors.

## CORS Configuration
Configure CORS with a named policy:

```csharp
// In Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiCorsPolicy", policy =>
    {
        policy.WithOrigins("https://allowed-origin.com")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("Location");
    });
});

// In middleware pipeline
app.UseCors("ApiCorsPolicy");
```

## Program.cs Patterns
Service registration (required):
- Configure JSON options: camelCase and `JsonStringEnumConverter`.
- Infrastructure: `IDbConnectionFactory` (singleton), `ISqlQueryLoader` (singleton), `IUnitOfWork` (scoped).
- Repositories: register all `I{{EntityName}}Repository` to `{{EntityName}}Repository` (scoped).
- AutoMapper: add all mapping profiles (e.g., `{{EntityName}}MappingProfile`).
- Handlers: register all command/query handlers (scoped).
- Health checks: add `LivenessCheck` and `ReadinessCheck`.
- Authentication: call `AddActiveDirectoryAuthentication(configuration)`.

Pipeline steps (in order):
- In Development: enable Swagger + SwaggerUI.
- `UseHttpsRedirection()`.
- `UseMiddleware<ExceptionHandlingMiddleware>()`.
- `UseCors("ApiCorsPolicy")`.
- `UseAuthentication()` (and authorization if added later).
- Map endpoint groups via `app.Map{{EntityName}}Endpoints()`.
- Map health checks at `/health/live` (liveness only) and `/health/ready` (all).

## Middleware
Exception handling middleware guidelines:
- One global middleware that logs and returns a standardized 500 response.
- Do not leak stack traces. Use a generic message and structured logging.
- Response body shape must follow `ApiResponse<object>` with `Success=false` and an error message.
Skeleton:
```csharp
public async Task InvokeAsync(HttpContext context)
{
    try { await _next(context); }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception");
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync(JsonSerializer.Serialize(new ApiResponse<object>
        {
            Success = false,
            ErrorMessage = "An internal server error occurred"
        }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
```

## Authentication & Authorization
Guidelines for token-based authentication:

1. **JWT Authentication**:
```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
    });
```

2. **For Active Directory Authentication**:
```csharp
public static IServiceCollection AddActiveDirectoryAuthentication(this IServiceCollection services, IConfiguration config)
{
    services.Configure<ActiveDirectorySettings>(config.GetSection("ActiveDirectory"));
    services.AddAuthentication("ActiveDirectory")
        .AddScheme<AuthenticationSchemeOptions, ActiveDirectoryAuthenticationHandler>("ActiveDirectory", null);
    return services;
}
```

3. **Applying Authorization to Endpoints**:
```csharp
app.MapGet("/protected", () => "Protected endpoint")
    .RequireAuthorization("PolicyName");
```

## Health Checks
Guidelines and minimal contracts:
- Liveness: returns Healthy if process is up.
- Readiness: verifies DB connection using `IDbConnectionFactory` and a simple `SELECT 1`.
Minimal examples:
```csharp
public class LivenessCheck : IHealthCheck
    => Task.FromResult(HealthCheckResult.Healthy("Application is running"));

public class ReadinessCheck : IHealthCheck
{
    private readonly IDbConnectionFactory _conn;
    public ReadinessCheck(IDbConnectionFactory conn) => _conn = conn;
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext _, CancellationToken __ = default)
    {
        try { using var c = _conn.CreateConnection(); c.Open(); using var cmd = c.CreateCommand(); cmd.CommandText = "SELECT 1"; cmd.ExecuteScalar(); return Task.FromResult(HealthCheckResult.Healthy("Ready")); }
        catch (Exception ex) { return Task.FromResult(HealthCheckResult.Unhealthy("Database connection failed", ex)); }
    }
}
```

## OpenAPI Documentation Rules
- Use `.WithName()`, `.WithSummary()`, `.WithDescription()` for each endpoint.
- Use `.Produces<T>(statusCode)` for all returned types/codes; include 400/404/500 where relevant.
- Use `.Accepts<T>("application/json")` for body inputs.
- Tags should match the route group entity plural.

## HTTP Status Rules
- 200 OK for successful GET/PUT.
- 201 Created for successful POST with Location header.
- 204 NoContent for successful DELETE.
- 400 BadRequest for validation/business errors.
- 404 NotFound when error contains "not found".
- 500 Internal Server Error for unexpected errors.

## Naming & Conventions
- Endpoint classes: `{{EntityName}}Endpoints`.
- Map extension: `Map{{EntityName}}Endpoints`.
- Routes: lowercase plural `/{{entityNamePlural}}`.
- Primary key suffix: `ID` (e.g., `{{EntityName}}ID`).
- All methods static; file-scoped namespaces.

## Dependencies
- Microsoft.AspNetCore.OpenApi v9.0.0
- Swashbuckle.AspNetCore v6.8.1
- Serilog.AspNetCore v8.0.3
- Microsoft.Extensions.Diagnostics.HealthChecks v9.0.0
- AspNetCore.HealthChecks.SqlServer v8.0.2
- AspNetCore.HealthChecks.UI.Client v9.0.0
- Microsoft.AspNetCore.Authentication.JwtBearer v9.0.0 (for JWT auth)

## Prohibited Patterns
- No controllers.
- No MediatR. Use custom CQRS handler classes injected directly.
- No raw results; always `ApiResponse<T>` + `TypedResults`.
- Do not throw for business errors; rely on `Result<T>` mapping.
- No inline SQL in API layer.

## Integration Points
- Logic layer: inject command/query handlers directly.
- Common layer: `Result<T>`, `PaginatedResult<T>`, `ApiResponse<T>`, error constants.
- Data layer: infrastructure-only usage (e.g., health checks), not business logic.