# AGENT.md - Data Layer

## Purpose
Data layer patterns for entities, repositories, and SQL queries using Dapper ORM with Clean Architecture principles.

## Entity Structure Patterns
- All entities MUST inherit from BaseEntity which provides audit fields: IsActive, CreatedDate, ModifiedDate, ModUser, Source.
- Entity classes should match OpenAPI entity definitions.

## Repository Pattern
- Use repository interfaces and implementations as described.
- Use IDbConnectionFactory and ISqlQueryLoader for all DB access.
- Return Result<T> or PaginatedResult<T> for all methods.
- Use structured logging and error handling.

## SQL Query File Organization
- Store all SQL queries in Sql/ folder, one file per operation.
- Use parameterized queries, soft delete, and naming conventions as described.

## Infrastructure, Error Handling, Dependencies, Prohibited Patterns, Integration Points
// ... (add all relevant sections from instructions for completeness) ...
