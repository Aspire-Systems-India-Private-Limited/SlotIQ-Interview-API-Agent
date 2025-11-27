---
applyTo: SlotIQ.Interview.Data/**/*.cs, SlotIQ.Interview.Data/**/*.sql
description: Data layer patterns for entities, repositories, and SQL queries using Dapper ORM with Clean Architecture principles
---

# Data Layer Instructions - SlotIQ Interview Solution

## Variable Resolution
| Variable               | Description                                 |
|------------------------|---------------------------------------------|
| `{{EntityName}}`        | Singular name of the entity (e.g., Member) |
| `{{EntityNamePlural}}`  | Plural form (e.g., Members)                |

## Entity Structure Patterns
- Generate Entity classes based on MemberManagmentEntities.openapi.yaml(./Requirement/Technical/MemberManagmentEntities.openapi.yaml)

### BaseEntity Pattern
All entities MUST inherit from BaseEntity which provides audit fields:
- IsActive (boolean) - For soft delete pattern
- CreatedDate (DateTime)
- ModifiedDate (DateTime)
- CreatedBy (string)
- ModifiedBy (string)
- Source (string)

### Entity Implementation Pattern
```csharp
// filepath: SlotIQ.Interview.Data.Entities/{{EntityName}}.cs
public class {{EntityName}} : BaseEntity
{
    public Guid {{EntityName}}ID { get; set; }  // CRITICAL: Use ID suffix, not Id
    public string PropertyName { get; set; } = string.Empty;
    public string? OptionalProperty { get; set; }
    public SomeEnum EnumProperty { get; set; }
}
```

### Entity Relationships
When implementing relationships between entities:

#### One-to-Many Relationship
```csharp
// Parent entity
public class Department : BaseEntity
{
    public Guid DepartmentID { get; set; }
    public string Name { get; set; } = string.Empty;
}

// Child entity with foreign key
public class Employee : BaseEntity
{
    public Guid EmployeeID { get; set; }
    public Guid DepartmentID { get; set; }  // Foreign key to Department
    public string Name { get; set; } = string.Empty;
}
```

## Repository Pattern

### Repository Interface Pattern
```csharp
public interface I{{EntityName}}Repository
{
    Task<Result<{{EntityName}}>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<{{EntityName}}>>> GetAllAsync();
    Task<PaginatedResult<{{EntityName}}>> GetPagedAsync(int pageNumber, int pageSize);
    Task<Result<{{EntityName}}>> AddAsync({{EntityName}} entity);
    Task<Result<{{EntityName}}>> UpdateAsync({{EntityName}} entity);
    Task<Result<bool>> DeleteAsync(Guid id);
}
```

### Repository Implementation Pattern 
```csharp
public class {{EntityName}}Repository : I{{EntityName}}Repository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ISqlQueryLoader _queryLoader;
    private readonly ILogger<{{EntityName}}Repository> _logger;

    // Constructor with dependency injection
    public {{EntityName}}Repository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader, 
        ILogger<{{EntityName}}Repository> logger)
    {
        _connectionFactory = connectionFactory;
        _queryLoader = queryLoader;
        _logger = logger;
    }

    public async Task<Result<{{EntityName}}>> GetByIdAsync(Guid id)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = _queryLoader.LoadQuery("Get{{EntityName}}ById");
            
            var entity = await connection.QueryFirstOrDefaultAsync<{{EntityName}}>(query, new { Id = id });
            
            if (entity == null)
                return Result<{{EntityName}}>.Failure(ErrorMessages.{{EntityName}}NotFound);
            
            return Result<{{EntityName}}>.Success(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving {{EntityName}} with ID {Id}", id);
            return Result<{{EntityName}}>.Failure(ErrorMessages.DatabaseError);
        }
    }

    // Additional CRUD methods follow same pattern
}
```

**Key Patterns:**
- Use `IDbConnectionFactory` for connection management
- Load SQL from external files via `ISqlQueryLoader`
- Return `Result<T>` or `PaginatedResult<T>`
- Structured logging with contextual information
- Set audit fields automatically (CreatedDate, ModifiedDate)

## SQL Query File Organization

### SQL File Naming Convention
Store all SQL queries in `SlotIQ.Interview.Data/Sql/` folder:
- `Get{{EntityName}}ById.sql`
- `GetAll{{EntityName}}s.sql`
- `Get{{EntityName}}sPaged.sql`
- `Insert{{EntityName}}.sql`
- `Update{{EntityName}}.sql`
- `Delete{{EntityName}}.sql`

### SQL Query Examples

#### Basic Queries
```sql
-- Get{{EntityName}}ById.sql
SELECT * FROM {{EntityName}} 
WHERE {{EntityName}}ID = @Id AND IsActive = 1

-- Delete{{EntityName}}.sql (Soft Delete)
UPDATE {{EntityName}} 
SET IsActive = 0, ModifiedDate = @ModifiedDate
WHERE {{EntityName}}ID = @Id
```

#### Complex Queries with Joins
```sql
-- GetEmployeesWithDepartments.sql
SELECT e.*, d.Name as DepartmentName
FROM Employees e
JOIN Departments d ON e.DepartmentID = d.DepartmentID
WHERE e.IsActive = 1 AND d.IsActive = 1
ORDER BY e.LastName, e.FirstName
```

#### Queries with Aggregations
```sql
-- GetDepartmentEmployeeCount.sql
SELECT d.DepartmentID, d.Name, COUNT(e.EmployeeID) as EmployeeCount
FROM Departments d
LEFT JOIN Employees e ON d.DepartmentID = e.DepartmentID AND e.IsActive = 1
WHERE d.IsActive = 1
GROUP BY d.DepartmentID, d.Name
```

**Key Rules:**
- Use parameterized queries to prevent SQL injection
- Include `IsActive = 1` filter for soft delete pattern
- Use soft delete (UPDATE IsActive = 0) instead of hard delete

## Infrastructure Components

### Unit of Work Pattern
The UnitOfWork pattern manages transactions across multiple repositories:

```csharp
public interface IUnitOfWork : IDisposable
{
    IDbConnection Connection { get; }
    IDbTransaction? Transaction { get; }
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
```

### Database Connection and SQL Query Management
- `IDbConnectionFactory`: Creates and manages database connections
- `ISqlQueryLoader`: Loads SQL queries from external .sql files

### Dependency Injection Configuration
```csharp
// In Program.cs - Service Registration
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddSingleton<ISqlQueryLoader, SqlQueryLoader>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
```

## Error Handling Patterns

### Structured Logging Examples
```csharp
_logger.LogInformation("Successfully retrieved {{EntityName}} with ID {Id}", id);
_logger.LogWarning("{{EntityName}} with ID {Id} not found", id);
_logger.LogError(ex, "Error retrieving {{EntityName}} with ID {Id}", id);
```

### Error Message Constants
```csharp
return Result<{{EntityName}}>.Failure(ErrorMessages.{{EntityName}}NotFound);
return Result<{{EntityName}}>.Failure(ErrorMessages.DatabaseError);
```

## Required Dependencies
- **Dapper**: For database operations
- **Microsoft.Data.SqlClient**: For SQL Server connectivity
- **Microsoft.Extensions.Logging**: For structured logging
- **Microsoft.Extensions.Configuration.Abstractions**: For configuration access
- **Microsoft.Extensions.DependencyInjection**: For dependency injection
- **SlotIQ.Interview.Common**: For Result<T>, ErrorMessages, and enums

## Prohibited Patterns
1. **NO Inline SQL**: Use external .sql files only
2. **NO Hard Deletes**: Use soft delete with IsActive flag
3. **NO Direct Exceptions**: Use Result<T> pattern
4. **NO Magic Strings**: Use constants from ErrorMessages
5. **NO Entity Framework**: Use Dapper only

## Integration Points
- **Logic Layer**: Repository interfaces are implemented here
- **Common Layer**: Uses Result<T>, PaginatedResult<T>, and ErrorMessages
- **API Layer**: No direct dependency (access through Logic layer only)