# AGENT.md - Integration Layer

## Purpose
Layer-specific instructions for implementing external service clients with resilience and error handling in the Integration project.

## Core Principles
- Interface-based design for all service clients (e.g., IPaymentGatewayClient)
- Use Polly for resilience (retry, circuit breaker, timeout)
- Always return Result<T> for error handling (never throw directly)
- Structured logging for all external service interactions
- File-scoped namespaces in all C# files

## Service Client Pattern

### Interface Example
```csharp
public interface I{{ServiceName}}Client
{
    Task<Result<TResponse>> GetAsync<TResponse>(string endpoint, CancellationToken ct = default)
        where TResponse : class;
    Task<Result<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken ct = default)
        where TRequest : class
        where TResponse : class;
}
```

### Implementation Example
- Use HttpClient injected via DI
- Apply Polly policies for all outbound calls
- Log all requests and responses (with correlation IDs)
- Map external errors to Result<T> failure

## Prohibited Patterns
- No direct exception propagation
- No hard-coded endpoints or credentials
- No synchronous HTTP calls

## Integration Points
- Logic layer: inject service client interfaces
- Common layer: use Result<T> and error constants

---

> This file is for use by the GitHub Coding Agent and all contributors to ensure consistent, resilient, and testable integration with external services in the Integration layer.
