# AGENT.md - UnitTests Layer

## Purpose
Defines comprehensive unit testing patterns using xUnit, Moq, and FluentAssertions. Tests validate repository implementations, command/query handlers, and API endpoints with proper isolation and mocking.

## Core Principles
- Test-First Development: Tests created before or alongside implementation
- 95% Coverage Minimum: Non-negotiable coverage requirement
- Test isolation: each test independent, no dependencies between tests
- Mock all external dependencies (repositories, loggers, mappers)
- FluentAssertions for expressive, readable assertions
- Async/await for all async operation tests
- Arrange-Act-Assert (AAA) pattern
- File-scoped namespaces in all test files
- Meaningful test data, no hard-coded values
- Simple, focused test logic with meaningful assertions

## Test Organization (MANDATORY STRUCTURE)
// ... (add test folder structure and organization as in instructions) ...

## Patterns & Guidelines
- Use xUnit for all tests.
- Use Moq for mocking dependencies.
- Use FluentAssertions for assertions.
- Cover all business logic and edge cases.
- Use in-memory SQLite for repository tests.

## Prohibited Patterns
- No integration tests in UnitTests project.
- No hard-coded test data; use builders or factories.

## Integration Points
- Data layer: test repositories with in-memory DB
- Logic layer: test handlers with mocks
- API layer: test endpoints with mocked handlers
