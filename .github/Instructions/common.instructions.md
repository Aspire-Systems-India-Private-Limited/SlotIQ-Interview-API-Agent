---
applyTo: SlotIQ.Interview.Common/**/*.cs
description: Common layer patterns for shared types, constants, enums, and models used across all layers
---

# Common Layer Instructions - SlotIQ Interview Solution

## Purpose
Define shared types, constants, enums, and models used across all layers with no business logic.

## Variable Resolution
| Variable               | Description                                 |
|------------------------|---------------------------------------------|
| `{{EntityName}}`        | Singular name of the entity (e.g., Member) |
| `{{EntityNamePlural}}`  | Plural form (e.g., Members)                |
| `{{ProjectPrefix}}`     | Project namespace prefix                    |

## Critical Patterns

### 1. Enums with JSON Serialization
When adding new enums:
- Always use the `[JsonConverter(typeof(JsonStringEnumConverter))]` attribute
- Use PascalCase for enum values
- Place in the `SlotIQ.Interview.Common.Enums` namespace

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

### 2. Result Pattern Extensions
When extending the Result pattern:
- Maintain immutability of Result<T> objects
- Add new extension methods to ResultExtensions class
- Support both synchronous and asynchronous operations
- Follow functional composition patterns (Map, Bind)

### 3. Response Models
When adding new response types:
- Follow the existing pattern of ApiResponse<T> and PaginatedResult<T>
- Include appropriate conversion methods between types
- Maintain consistent property naming with existing models
- Implement proper validation for pagination parameters

### 4. Error Message Constants
When adding new error messages to ErrorMessages class:
- Group by category (General, Entity-specific, Validation)
- Use descriptive constant names that indicate purpose
- Follow the pattern: `{EntityName}NotFound`, `Invalid{PropertyName}`
- Include context in the message text to aid troubleshooting

Examples:
```csharp
// Adding entity-specific error messages
public const string ScheduleNotFound = "Schedule not found.";
public const string DuplicateSchedule = "A schedule with this name already exists.";

// Adding validation error messages
public const string InvalidScheduleDate = "Invalid schedule date format.";
public const string RequiredScheduleName = "Schedule name is required.";
```

## Implementation Guidelines

### Naming Conventions
- Use PascalCase for all public types and members
- Use camelCase for private fields, prefixed with underscore
- Follow standard C# naming conventions for interfaces (IMyInterface)

### Code Organization
- Maintain separation between different types of models
- Group related functionality in appropriate namespaces
- Place extension methods in the Helpers folder
- Keep models immutable where possible

### Performance Considerations
- Use System.Text.Json instead of Newtonsoft.Json
- Consider using record types for DTOs
- Be mindful of memory usage in collection processing
- Avoid excessive object nesting in response models

## Prohibited Patterns
1. NO business logic in the common layer
2. NO external dependencies beyond what's necessary
3. NO magic strings (use constants)
4. NO mutable static state
5. NO direct coupling to data or API layers

## Reference Implementation
See existing files in the solution for implementation examples:
- `SlotIQ.Interview.Common.Models.Result<T>` - For the Result pattern
- `SlotIQ.Interview.Common.Helpers.ResultExtensions` - For Result extensions
- `SlotIQ.Interview.Common.Models.PaginatedResult<T>` - For pagination
- `SlotIQ.Interview.Common.Constants.ErrorMessages` - For error messages