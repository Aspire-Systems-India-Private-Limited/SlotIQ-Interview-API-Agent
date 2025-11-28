# AGENT.md - Logic Layer

## Purpose
Defines custom CQRS patterns for business logic. Contains command/query handlers, DTOs, and AutoMapper configurations. No MediatR - uses record types and generic interfaces for commands/queries with direct handler injection.

## Core Principles
- Custom CQRS implementation. No MediatR.
- Record types for immutable commands/queries.
- Generic interfaces specify return types.
- All handlers return `Result<T>` or `PaginatedResult<T>`.
- Transaction management via `IUnitOfWork` for commands only.
- AutoMapper for entity-DTO mapping.
- File-scoped namespaces in all C# files.
- Primary keys use `{{EntityName}}ID` (ID suffix, not Id).

## CQRS Interfaces & Patterns
// ... (add CQRS interface, command/query/handler, DTO, validation, mapping, error handling, naming, dependencies, prohibited patterns, integration points as in instructions) ...
