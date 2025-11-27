---
applyTo: SlotIQ.Interview.Integration/**/*.cs
description: Integration layer patterns for external service clients with resilience and error handling
---

# Integration Layer Instructions - SlotIQ Interview Solution

## Purpose
Defines external service integration patterns for third-party APIs using adapter pattern, resilience policies, and consistent error handling.

## Variables
| Variable         | Description                                |
|------------------|--------------------------------------------|
| `{{EntityName}}` | Singular entity name (e.g., Member)        |
| `{{ServiceName}}`| External service name (e.g., PaymentGateway) |

## Core Principles
- Interface-based design for testability
- Resilience with Polly policies (retry, circuit breaker, timeout)
- Result<T> pattern for error handling
- Structured logging of external interactions
- File-scoped namespaces in all C# files

## Service Client Pattern

### Interface
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

### Implementation
```csharp
public class {{ServiceName}}Client : I{{ServiceName}}Client
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<{{ServiceName}}Client> _logger;
    private readonly IOptions<{{ServiceName}}Options> _options;
    
    // Constructor with dependencies
    
    public async Task<Result<TResponse>> GetAsync<TResponse>(string endpoint, CancellationToken ct = default)
        where TResponse : class
    {
        try
        {
            _logger.LogInformation("Calling {{ServiceName}} GET: {Endpoint}", endpoint);
            var response = await _httpClient.GetAsync(endpoint, ct);
            return await ProcessResponseAsync<TResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling {{ServiceName}} GET: {Endpoint}", endpoint);
            return Result<TResponse>.Failure(ErrorMessages.ExternalServiceError);
        }
    }
    
    // Additional methods and ProcessResponseAsync implementation
}
```

## Resilience Configuration
```csharp
// In service registration
services.AddHttpClient<I{{ServiceName}}Client, {{ServiceName}}Client>()
    .AddPolicyHandler((provider, _) => 
    {
        var options = provider.GetRequiredService<IOptions<{{ServiceName}}Options>>().Value;
        var logger = provider.GetRequiredService<ILogger<{{ServiceName}}Client>>();
        
        return Policy.WrapAsync(
            CreateRetryPolicy(options, logger),
            CreateCircuitBreakerPolicy(options, logger),
            CreateTimeoutPolicy(options, logger));
    });
```

## Adapter Pattern
```csharp
public interface I{{ServiceName}}Service
{
    Task<Result<DomainResponseDto>> ExecuteOperationAsync(DomainRequestDto request, CancellationToken ct = default);
}

public class {{ServiceName}}Service : I{{ServiceName}}Service
{
    private readonly I{{ServiceName}}Client _client;
    
    // Implementation with mapping between domain and external service models
}
```

## Error Handling Strategy
- Map HTTP status codes to appropriate error messages
- Catch network errors as ExternalServiceUnavailable
- Catch timeouts as ExternalServiceTimeout
- Use ErrorMessages constants for consistent error reporting

## Required Dependencies
- Microsoft.Extensions.Http + Polly
- System.Text.Json
- Microsoft.Extensions.Logging.Abstractions
- Microsoft.Extensions.Options.ConfigurationExtensions

## Prohibited Patterns
1. NO direct HttpClient usage (use factory)
2. NO direct exception propagation (use Result<T>)
3. NO hard-coded URLs or credentials
4. NO service-specific models in domain logic
5. NO missing resilience policies
6. NO synchronous calls
        {
            _logger.LogError(ex, "Error calling {{ServiceName}} POST: {Endpoint}", endpoint);
            return Result<TResponse>.Failure(ErrorMessages.ExternalServiceUnavailable);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error calling {{ServiceName}} POST: {Endpoint}", endpoint);
            return Result<TResponse>.Failure(ErrorMessages.ExternalServiceError);
        }
    }
    
    private async Task<Result<TResponse>> ProcessResponseAsync<TResponse>(HttpResponseMessage response)
        where TResponse : class
    {
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });
            
            return result != null
                ? Result<TResponse>.Success(result)
                : Result<TResponse>.Failure(ErrorMessages.DeserializationError);
        }
        
        var errorContent = await response.Content.ReadAsStringAsync();
        _logger.LogWarning("{{ServiceName}} returned error status code {StatusCode}: {ErrorContent}",
            (int)response.StatusCode, errorContent);
            
        // Map HTTP status codes to appropriate error messages
        return response.StatusCode switch
        {
            HttpStatusCode.NotFound => Result<TResponse>.Failure(ErrorMessages.ResourceNotFound),
            HttpStatusCode.Unauthorized => Result<TResponse>.Failure(ErrorMessages.AuthenticationFailed),
            HttpStatusCode.Forbidden => Result<TResponse>.Failure(ErrorMessages.AccessDenied),
            HttpStatusCode.BadRequest => Result<TResponse>.Failure(errorContent),
            _ => Result<TResponse>.Failure(ErrorMessages.ExternalServiceError)
        };
    }
}
```

## Service Options Pattern
```csharp
public class {{ServiceName}}Options
{
    public const string SectionName = "ExternalServices:{{ServiceName}}";
    
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 30;
    public int RetryCount { get; set; } = 3;
    public int CircuitBreakerThreshold { get; set; } = 5;
    public int CircuitBreakerDurationSeconds { get; set; } = 30;
}
```

## Resilience Patterns with Polly

### Circuit Breaker Pattern
```csharp
public static class ResiliencePolicies
{
    public static IAsyncPolicy<HttpResponseMessage> CreateCircuitBreakerPolicy({{ServiceName}}Options options, ILogger logger)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                options.CircuitBreakerThreshold,
                TimeSpan.FromSeconds(options.CircuitBreakerDurationSeconds),
                onBreak: (_, timespan) =>
                {
                    logger.LogWarning("Circuit breaker for {{ServiceName}} opened for {TimeSpan} seconds", timespan.TotalSeconds);
                },
                onReset: () =>
                {
                    logger.LogInformation("Circuit breaker for {{ServiceName}} reset");
                });
    }
    
    public static IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy({{ServiceName}}Options options, ILogger logger)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                options.RetryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryAttempt, _) =>
                {
                    logger.LogWarning("Retrying {{ServiceName}} request after {TimeSpan}s (attempt {RetryAttempt})", timespan.TotalSeconds, retryAttempt);
                });
    }
    
    public static IAsyncPolicy<HttpResponseMessage> CreateTimeoutPolicy({{ServiceName}}Options options, ILogger logger)
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(
            TimeSpan.FromSeconds(options.TimeoutSeconds),
            TimeoutStrategy.Pessimistic,
            onTimeoutAsync: (_, timespan, _, _) =>
            {
                logger.LogWarning("Request to {{ServiceName}} timed out after {TimeSpan}s", timespan.TotalSeconds);
                return Task.CompletedTask;
            });
    }
    
    public static IAsyncPolicy<HttpResponseMessage> CreateCombinedPolicy({{ServiceName}}Options options, ILogger logger)
    {
        return Policy.WrapAsync(
            CreateRetryPolicy(options, logger),
            CreateCircuitBreakerPolicy(options, logger),
            CreateTimeoutPolicy(options, logger));
    }
}
```

## Dependency Injection Configuration
```csharp
// In Program.cs or ServiceCollectionExtensions.cs
public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
{
    // Register service options
    services.Configure<{{ServiceName}}Options>(configuration.GetSection({{ServiceName}}Options.SectionName));
    
    // Register HTTP client with resilience policies
    services.AddHttpClient<I{{ServiceName}}Client, {{ServiceName}}Client>()
        .AddPolicyHandler((serviceProvider, _) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<{{ServiceName}}Options>>().Value;
            var logger = serviceProvider.GetRequiredService<ILogger<{{ServiceName}}Client>>();
            return ResiliencePolicies.CreateCombinedPolicy(options, logger);
        });
        
    return services;
}
```

## Error Handling Strategy
Follow these patterns for consistent error handling:

1. **Network Errors**: Catch `HttpRequestException` and return `ErrorMessages.ExternalServiceUnavailable`
2. **Timeout Errors**: Catch `TaskCanceledException` and return `ErrorMessages.ExternalServiceTimeout`
3. **Authentication Failures**: Map 401 responses to `ErrorMessages.AuthenticationFailed`
4. **Authorization Failures**: Map 403 responses to `ErrorMessages.AccessDenied`
5. **Resource Not Found**: Map 404 responses to `ErrorMessages.ResourceNotFound`
6. **Bad Requests**: Map 400 responses to error details from response body
7. **Server Errors**: Map 500+ responses to `ErrorMessages.ExternalServiceError`
8. **Deserialization Errors**: Return `ErrorMessages.DeserializationError`

## Logging Strategy
Log all external service interactions:
- Request details (endpoint, method, request ID)
- Response status and timing
- Error details with appropriate log level
- Circuit breaker state changes
- Retry attempts

```csharp
// Request logging
_logger.LogInformation("Calling {{ServiceName}} {Method}: {Endpoint} [RequestId: {RequestId}]", 
    "POST", endpoint, Guid.NewGuid());

// Response logging
_logger.LogInformation("{{ServiceName}} response received in {ElapsedMs}ms [StatusCode: {StatusCode}]", 
    stopwatch.ElapsedMilliseconds, (int)response.StatusCode);
    
// Error logging
_logger.LogError(ex, "Error calling {{ServiceName}} {Method}: {Endpoint} [RequestId: {RequestId}]", 
    "POST", endpoint, requestId);
```

## Adapter Pattern for External Service Models

### External Service DTOs
Place all external service DTOs in a dedicated namespace:
```csharp
namespace SlotIQ.Interview.Integration.Models.{{ServiceName}};

public record {{ServiceName}}RequestDto
{
    [JsonPropertyName("requestId")]
    public string RequestId { get; init; } = Guid.NewGuid().ToString();
    
    [JsonPropertyName("payload")]
    public required object Payload { get; init; }
    
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; init; } = DateTime.UtcNow.ToString("o");
}

public record {{ServiceName}}ResponseDto
{
    [JsonPropertyName("responseId")]
    public string ResponseId { get; init; } = string.Empty;
    
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;
    
    [JsonPropertyName("data")]
    public object? Data { get; init; }
    
    [JsonPropertyName("errors")]
    public List<string>? Errors { get; init; }
}
```

### Domain Service Adapter
Create an adapter layer between external services and domain logic:
```csharp
public interface I{{ServiceName}}Service
{
    Task<Result<DomainResponseDto>> ExecuteDomainOperationAsync(DomainRequestDto request, CancellationToken ct = default);
}

public class {{ServiceName}}Service : I{{ServiceName}}Service
{
    private readonly I{{ServiceName}}Client _client;
    private readonly ILogger<{{ServiceName}}Service> _logger;
    
    public {{ServiceName}}Service(I{{ServiceName}}Client client, ILogger<{{ServiceName}}Service> logger)
    {
        _client = client;
        _logger = logger;
    }
    
    public async Task<Result<DomainResponseDto>> ExecuteDomainOperationAsync(DomainRequestDto request, CancellationToken ct = default)
    {
        // Map domain request to external service request
        var serviceRequest = MapToDtoRequest(request);
        
        // Call external service
        var result = await _client.PostAsync<{{ServiceName}}RequestDto, {{ServiceName}}ResponseDto>(
            "api/endpoint", serviceRequest, ct);
            
        if (!result.IsSuccess)
            return Result<DomainResponseDto>.Failure(result.Error);
            
        // Map external service response to domain response
        var domainResponse = MapToDomainResponse(result.Value);
        return Result<DomainResponseDto>.Success(domainResponse);
    }
    
    private {{ServiceName}}RequestDto MapToDtoRequest(DomainRequestDto request)
    {
        return new {{ServiceName}}RequestDto
        {
            Payload = new 
            {
                // Map properties from domain request to service request
                PropertyName = request.PropertyName,
                // Other mappings...
            }
        };
    }
    
    private DomainResponseDto MapToDomainResponse({{ServiceName}}ResponseDto response)
    {
        return new DomainResponseDto
        {
            // Map properties from service response to domain response
            Success = response.Status == "success",
            Data = response.Data,
            ErrorMessages = response.Errors
        };
    }
}
```

## Required Dependencies
- **Microsoft.Extensions.Http**: For HttpClient factory
- **Microsoft.Extensions.Http.Polly**: For resilience policies
- **Polly**: For circuit breaker, retry, and timeout policies
- **System.Text.Json**: For JSON serialization/deserialization
- **Microsoft.Extensions.Logging.Abstractions**: For logging
- **Microsoft.Extensions.Options.ConfigurationExtensions**: For options pattern
- **SlotIQ.Interview.Common**: For Result<T> and error messages

## Prohibited Patterns
1. **NO Direct HttpClient Usage**: Always use registered HttpClient through DI
2. **NO Direct Exception Propagation**: Use Result<T> pattern for all operations
3. **NO Hard-Coded URLs or Credentials**: Use options pattern
4. **NO Service-Specific Models in Domain Logic**: Use adapters and mappers
5. **NO Missing Resilience Policies**: Always include retry, circuit breaker, and timeout
6. **NO Synchronous Calls**: Use async/await for all external service calls

## Integration Points
- **Logic Layer**: Inject service interfaces into command/query handlers
- **Common Layer**: Use Result<T> pattern and error messages
- **API Layer**: No direct dependency (access through Logic layer only)
