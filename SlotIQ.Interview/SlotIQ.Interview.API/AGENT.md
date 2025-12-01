# AGENT.md - API Layer

## Purpose
Defines Minimal API endpoints, middleware, health checks, and authentication. The API layer accepts HTTP requests, validates and documents them, and delegates to CQRS handlers in the Logic layer. It returns consistent JSON responses using `ApiResponse<T>` and `TypedResults`.

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

## Endpoint Structure & Patterns
- File: `SlotIQ.Interview.API/Endpoints/{{EntityName}}Endpoints.cs`
- Class: `{{EntityName}}Endpoints`
- Mapper: `Map{{EntityName}}Endpoints(this IEndpointRouteBuilder app)`
- Group: `/{{entityNamePlural}}` (lowercase plural) with `.WithTags("{{EntityNamePlural}}").WithOpenApi()`
- Document endpoints with `.WithName()`, `.WithSummary()`, `.WithDescription()`, `.Accepts<T>()`, `.Produces<T>()`
- Provide GET by ID, POST Create, PUT Update, DELETE, GET Paged as needed
- Inject the relevant handler directly as a parameter.
- Use the appropriate command/query record type.
- Translate `Result<T>` into `TypedResults` and `ApiResponse<T>`.
- Detect not-found using error message content contains "not found" (case-insensitive), otherwise 400 for business errors, 500 for unexpected errors.

## Canonical Endpoint Examples
// ... (add canonical GET, POST, PUT, DELETE, GET Paged endpoint patterns as in instructions) ...

## CORS, Program.cs, Middleware, Auth, Health Checks, OpenAPI, Status, Naming, Dependencies, Prohibited Patterns, Integration Points
// ... (add all relevant sections from instructions for completeness) ...
