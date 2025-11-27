# Code Review Workflow - SlotIQ Interview Solution

## Pre-Review Preparation

### Review Checklist
Before submitting code for review:
- Code compiles without errors or warnings
- All unit and integration tests pass
- No linting errors or warnings
- Self-reviewed changes for obvious issues
- Code comments and API docs updated
- Clear, descriptive commit messages
- Following proper Git branching conventions

### Layer-Specific Instruction Files
Before conducting a code review, refer to these layer-specific instruction files for detailed implementation patterns:
- **Common Layer**: [common.Instructions.md](/.github/Instructions/common.Instructions.md)
- **Data Layer**: [data.Instructions.md](/.github/Instructions/data.Instructions.md)
- **Logic Layer**: [logic.Instructions.md](/.github/Instructions/logic.Instructions.md)
- **API Layer**: [api.Instructions.md](/.github/Instructions/api.Instructions.md)
- **Integration Layer**: [integration.Instructions.md](/.github/Instructions/integration.Instructions.md)
- **Unit Testing**: [testing.Instructions.md](/.github/Instructions/testing.Instructions.md)

### Review Scope Classification
- **Feature Review**: New functionality implementation
- **Bugfix Review**: Issue resolution and fixes
- **Refactoring Review**: Code structure improvements
- **Hotfix Review**: Critical production fixes
- **Documentation Review**: Documentation-only changes

## Code Review Process

### Phase 1: Architecture and Design Review
**Review Criteria:**
- **Clean Architecture**: Proper layer separation and dependencies
- **CQRS Implementation**: Commands and queries follow patterns
- **Result Pattern**: All operations return Result<T> or PaginatedResult<T>
- **Dependency Flow**: API → Logic → Data → Common
- **Single Responsibility**: Each class has one clear purpose

**Architecture Red Flags:**
- ❌ Logic layer accessing Data layer directly
- ❌ Controllers instead of Minimal APIs
- ❌ MediatR usage (should use custom CQRS)
- ❌ Direct exceptions for business logic
- ❌ Circular dependencies between layers

### Phase 2: Layer-Specific Review

#### Common Layer Review
**Checklist:**
- **Enums**: Use JsonStringEnumConverter attribute
- **Result Pattern**: Proper Success/Failure factory methods
- **Error Messages**: Constants in ErrorMessages class
- **No Business Logic**: Only shared types and utilities
- **Naming Conventions**: PascalCase for classes and properties

> **Note:** Refer to [common.Instructions.md](/.github/Instructions/common.Instructions.md) for comprehensive Common layer patterns and requirements.

**Pattern Examples:**
```csharp
// ✅ Good: Proper enum with JSON converter
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MemberRole { Admin, User, Manager }

// ❌ Bad: Missing JSON converter
public enum MemberRole { Admin, User, Manager }
```

#### Data Layer Review
**Checklist:**
- **Entity Structure**: Inherits from BaseEntity
- **Primary Keys**: Use `{{EntityName}}ID` suffix (not `{{EntityName}}Id`)
- **Repository Pattern**: Interfaces in Contracts folder
- **SQL Queries**: External .sql files, no inline SQL
- **Parameterized Queries**: Prevent SQL injection
- **Soft Delete**: Use IsActive flag
- **Error Handling**: Return Result<T> with proper error messages
- **Logging**: Structured logging with context

> **Note:** Refer to [data.Instructions.md](/.github/Instructions/data.Instructions.md) for detailed Data layer patterns, SQL examples, and entity relationship guidance.

**Pattern Examples:**
```csharp
// ✅ Good: Proper primary key naming
public class Member : BaseEntity
{
    public Guid MemberID { get; set; }  // Correct: ID suffix
}

// ❌ Bad: Incorrect primary key naming
public class Member : BaseEntity
{
    public Guid MemberId { get; set; }  // Incorrect: Id suffix
}
```

#### Logic Layer Review
**Checklist:**
- **CQRS Pattern**: Commands and queries are record types
- **Handler Interfaces**: Use custom ICommandHandler/IQueryHandler
- **DTO Structure**: Main, Create, Update DTOs follow patterns
- **Validation**: FluentValidation rules for all DTOs
- **AutoMapper**: Proper entity-DTO mapping profiles
- **Transaction Management**: Command handlers use IUnitOfWork
- **No Transactions**: Query handlers don't use IUnitOfWork
- **Business Logic**: Proper separation of concerns

> **Note:** Refer to [logic.Instructions.md](/.github/Instructions/logic.Instructions.md) for comprehensive Logic layer patterns, CQRS implementation details, and handler guidelines.

**Pattern Examples:**
```csharp
// ✅ Good: Command as record type
public record CreateMemberCommand(CreateMemberDto Dto) : ICommand<Result<MemberDto>>;

// ❌ Bad: Command as class
public class CreateMemberCommand : ICommand<Result<MemberDto>>
{
    public CreateMemberDto Dto { get; set; } = null!;
}
```

#### API Layer Review
**Checklist:**
- **Minimal APIs**: Static endpoint methods, no controllers
- **Handler Injection**: Direct handler injection in endpoints
- **TypedResults**: Use TypedResults for HTTP responses
- **ApiResponse Wrapper**: All responses wrapped in ApiResponse<T>
- **Route Constraints**: Use `{id:guid}` pattern
- **OpenAPI Documentation**: Complete endpoint documentation
- **Error Handling**: Consistent error response format
- **Status Codes**: Appropriate HTTP status codes

> **Note:** Refer to [api.Instructions.md](/.github/Instructions/api.Instructions.md) for detailed API layer patterns, endpoint organization, and response formatting guidelines.

**Pattern Examples:**
```csharp
// ✅ Good: Static endpoint method with TypedResults
public static async Task<Results<Ok<ApiResponse<MemberDto>>, NotFound<ApiResponse<object>>>> GetMemberById(
    Guid id, GetMemberByIdQueryHandler handler, CancellationToken ct)
{
    var result = await handler.Handle(new GetMemberByIdQuery(id), ct);
    if (!result.IsSuccess)
        return result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true
            ? TypedResults.NotFound(new ApiResponse<object> { Success = false, ErrorMessage = result.Error })
            : TypedResults.StatusCode(500);
    return TypedResults.Ok(new ApiResponse<MemberDto> { Success = true, Data = result.Value });
}

// ❌ Bad: Controller-based approach
[ApiController]
public class MemberController : ControllerBase { /* Violates Minimal API pattern */ }
```

#### Unit Testing Review
**Checklist:**
- **Test Organization**: Tests mirror source structure
- **Naming Convention**: Clear test method names
- **AAA Pattern**: Arrange, Act, Assert structure
- **FluentAssertions**: Use for readable assertions
- **Mock Verification**: Verify mock interactions
- **Test Coverage**: Cover success and failure scenarios
- **Repository Tests**: Use in-memory SQLite
- **Handler Tests**: Mock dependencies properly
- **Endpoint Tests**: Mock handlers, test TypedResults

> **Note:** Refer to [testing.Instructions.md](/.github/Instructions/testing.Instructions.md) for comprehensive testing patterns, mocking approaches, and test organization guidelines.

**Pattern Examples:**
```csharp
// ✅ Good: Proper test structure with FluentAssertions
[Fact]
public async Task GetByIdAsync_ShouldReturnSuccess_WhenEntityExists()
{
    // Arrange
    var testEntity = new Member { MemberID = Guid.NewGuid(), UserName = "test@example.com" };
    await _connection.ExecuteAsync("INSERT INTO Members ...", testEntity);

    // Act
    var result = await _repository.GetByIdAsync(testEntity.MemberID);

    // Assert
    result.IsSuccess.Should().BeTrue();
    result.Value.Should().NotBeNull();
    result.Value.MemberID.Should().Be(testEntity.MemberID);
}

// ❌ Bad: Unclear test with basic assertions
[Fact]
public async Task Test1()
{
    var result = await _repository.GetByIdAsync(Guid.NewGuid());
    Assert.True(result.IsSuccess);
}
```

### Phase 3: Code Quality Review

#### Code Style and Standards
- **File Scoped Namespaces**: All C# files use file-scoped namespaces
- **Nullable Reference Types**: Proper use of nullable types
- **String Handling**: Use `string.Empty` for required strings, `string?` for optional
- **Async/Await**: Proper async patterns with CancellationToken
- **Using Statements**: Proper resource disposal
- **XML Documentation**: Public APIs have XML comments

**Pattern Examples:**
```csharp
// ✅ Good: File-scoped namespace
namespace SlotIQ.Interview.Data.Entities;
public class Member : BaseEntity { }

// ❌ Bad: Traditional namespace
namespace SlotIQ.Interview.Data.Entities
{
    public class Member : BaseEntity { }
}
```

#### Performance and Security
- **SQL Injection Prevention**: All queries parameterized
- **Memory Management**: Proper disposal of resources
- **Exception Handling**: Specific exception types caught
- **Logging Security**: No sensitive data in logs
- **Input Validation**: All inputs validated
- **Query Efficiency**: No N+1 query problems

#### Error Handling and Logging
- **Result Pattern**: Consistent use of Result<T>
- **Error Messages**: Use ErrorMessages constants
- **Structured Logging**: Contextual information included
- **Exception Context**: Meaningful error messages
- **Log Levels**: Appropriate log levels used

### Phase 4: Integration and Dependencies

#### Dependency Injection
- **Service Registration**: All services registered in Program.cs
- **Lifetime Management**: Appropriate service lifetimes
- **Interface Segregation**: Proper interface design
- **Circular Dependencies**: No circular dependency issues

#### Configuration and Settings
- **Configuration Patterns**: Proper use of IConfiguration
- **Environment Specific**: Environment-specific settings
- **Connection Strings**: Secure connection string management
- **Health Checks**: Appropriate health check configuration

## Review Quality Gates

### Mandatory Requirements (Must Fix)
- **Compilation**: Code must compile without errors
- **Tests**: All tests must pass
- **Architecture**: Must follow Clean Architecture patterns
- **Security**: No security vulnerabilities
- **Performance**: No obvious performance issues

### Strong Recommendations (Should Fix)
- **Code Style**: Follow established conventions
- **Documentation**: Code should be well-documented
- **Test Coverage**: Adequate test coverage
- **Error Handling**: Comprehensive error handling
- **Logging**: Appropriate logging levels

### Suggestions (Nice to Have)
- **Code Simplification**: Opportunities for cleaner code
- **Performance Optimization**: Minor performance improvements
- **Readability**: Code readability enhancements
- **Maintainability**: Future maintainability considerations

## Common Review Feedback Patterns

### Architecture Violations
```csharp
// ❌ Problem: Logic layer accessing Data layer directly
public class SomeService
{
    private readonly MemberRepository _repository; // Should use interface
}

// ✅ Solution: Use interface from Contracts
public class SomeService
{
    private readonly IMemberRepository _repository;
}
```

### Error Handling Issues
```csharp
// ❌ Problem: Throwing exceptions for business logic
public async Task<Member> GetMemberAsync(Guid id)
{
    var member = await _repository.GetByIdAsync(id);
    if (member == null)
        throw new NotFoundException("Member not found");
    return member;
}

// ✅ Solution: Use Result pattern
public async Task<Result<Member>> GetMemberAsync(Guid id)
{
    var result = await _repository.GetByIdAsync(id);
    if (!result.IsSuccess)
        return Result<Member>.Failure(result.Error ?? ErrorMessages.MemberNotFound);
    return Result<Member>.Success(result.Value);
}
```

## Review Completion Checklist

### Before Approval
- **All Issues Addressed**: Critical and high-priority issues resolved
- **Tests Updated**: Tests reflect code changes
- **Documentation Updated**: API docs and code comments current
- **Performance Verified**: No performance regressions
- **Security Reviewed**: No security vulnerabilities introduced

### Post-Review Actions
- **Merge Strategy**: Appropriate merge/squash strategy
- **Deployment Plan**: Consider deployment impact
- **Monitoring**: Plan for post-deployment monitoring
- **Rollback Plan**: Rollback strategy if issues arise

## Review Tools and Automation

### Automated Checks
- **Build Pipeline**: Automated build and test execution
- **Code Analysis**: Static code analysis tools
- **Security Scanning**: Automated security vulnerability scanning
- **Test Coverage**: Code coverage reporting
- **Performance Testing**: Automated performance regression testing

### Manual Review Focus
- **Business Logic**: Correctness of business rules
- **Architecture Compliance**: Adherence to Clean Architecture
- **Code Readability**: Maintainability and clarity
- **Edge Cases**: Handling of boundary conditions
- **Integration Points**: Proper handling of external dependencies

## Quick Review Template

For efficient reviews, follow this sequence:
1. **Architecture Check**: Verify Clean Architecture compliance
2. **Layer Review**: Check layer-specific patterns and conventions (refer to layer-specific instruction files)
3. **Quality Gates**: Ensure mandatory requirements are met
4. **Performance & Security**: Review for performance and security issues
5. **Documentation**: Verify documentation is complete and accurate
6. **Testing**: Ensure adequate test coverage and quality