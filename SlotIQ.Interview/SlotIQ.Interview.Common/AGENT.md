# AGENT.md - Common Layer

## Purpose
Defines shared types, constants, enums, and models used across all layers with no business logic.

## Patterns & Guidelines

### 1. Enums with JSON Serialization
- Always use the `[JsonConverter(typeof(JsonStringEnumConverter))]` attribute
- Use PascalCase for enum values
- Place enums in the `SlotIQ.Interview.Common.Enums` namespace

Example:
```csharp
// filepath: SlotIQ.Interview.Common.Enums/{{EntityName}}Status.cs
using System.Text.Json.Serialization;

namespace SlotIQ.Interview.Common.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum {{EntityName}}Status
{
    Active,
    Inactive,
    Pending
}
```

### 2. Result Pattern
- Use `Result<T>` and `PaginatedResult<T>` for all operation results
- Use `ApiResponse<T>` for consistent API responses
- Use extension methods for Result<T> operations

### 3. Constants
- Centralize error messages and other constants in the `Constants` folder
- Use `ErrorMessages` for all error handling

### 4. File Organization
- Place shared models in `Models/`
- Place enums in `Enums/`
- Place constants in `Constants/`
- Place helpers/extensions in `Helpers/`

## Prohibited Patterns
- No business logic in the Common layer
- No direct dependencies on Data, Logic, or API layers

---

> This file is for use by the GitHub Coding Agent and all contributors to ensure consistent, high-quality implementation in the Common layer.
