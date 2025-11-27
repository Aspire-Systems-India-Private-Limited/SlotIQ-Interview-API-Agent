---
applyTo: SlotIQ.Interview.UnitTests/**/*.cs
description: Unit testing patterns for comprehensive test coverage using xUnit, Moq, and FluentAssertions with in-memory SQLite for repository tests
---

## Purpose
Defines comprehensive unit testing patterns using xUnit, Moq, and FluentAssertions. Tests validate repository implementations, command/query handlers, and API endpoints with proper isolation and mocking.

## Variables
- `{{EntityName}}`: singular entity (e.g., AvailabilityPlan)
- `{{EntityNamePlural}}`: plural entity (e.g., AvailabilityPlans)
- `{{ProjectPrefix}}`: root namespace (e.g., SlotIQ.Interview)

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
```
{{ProjectPrefix}}.UnitTests/
├── Tests/
│   ├── DataAccess/             # Data access tests with in-memory database
│   │   └── {{EntityName}}RepositoryTests.cs
│   ├── BusinessLogic/          # Business logic tests with mocks
│   │   ├── Commands/           # Command handler tests with mocks
│   │   │   └── Create{{EntityName}}CommandHandlerTests.cs
│   │   └── Queries/            # Query handler tests with mocks
│   │       └── Get{{EntityName}}ByIdQueryHandlerTests.cs
│   ├── Controllers/            # API/Controller tests with mocked services
│   │   └── {{EntityName}}ControllerTests.cs
│   ├── Validators/             # Validation tests
│   │   └── Create{{EntityName}}ValidatorTests.cs
│   └── Mapping/                # Mapping/transformation tests
│       └── {{EntityName}}MappingTests.cs
```

## Test Data Generation
- **Fixed Test Data**: Create helper methods for common test entities
- **Parameterized Test Data**: Use `[Theory]` and `[InlineData]` for multiple scenarios
- **Random Test Data**: Use generator methods for large datasets or stress testing

## Command Handler Testing
Test structure:
- Mock dependencies: `IUnitOfWork`, `I{{EntityName}}Repository`, `IMapper`, `ILogger<T>`
- Test success and failure scenarios
- Verify transaction flow (begin, commit/rollback)

## Query Handler Testing
Test structure:
- Mock dependencies: `I{{EntityName}}Repository`, `IMapper`, `ILogger<T>`
- No transaction management (read-only)
- Test success and not-found scenarios

## Repository Testing
Use in-memory SQLite:
- `SQLiteConnection("DataSource=:memory:")`
- Create table schema matching entity structure
- Setup SQL query mocks via `ISqlQueryLoader`
- Test CRUD operations with real database interactions

## API Endpoint Testing
Test static endpoint methods:
- Mock command/query handlers
- Test success and error scenarios
- Verify correct HTTP status codes and response types

## Code Coverage Requirements
- Aim for minimum 80% code coverage across all projects
- 100% coverage for critical business logic
- Coverage report generated as part of CI pipeline

## Test Naming & Structure
Test class naming:
- `{{EntityName}}RepositoryTests`
- `Create{{EntityName}}CommandHandlerTests`
- `Get{{EntityName}}ByIdQueryHandlerTests`
- `{{EntityName}}EndpointsTests`

Test method naming: `MethodName_Condition_ExpectedResult`
- `Handle_ValidCommand_ReturnsSuccess`
- `GetByIdAsync_EntityNotFound_ReturnsFailure`
- `Create{{EntityName}}_InvalidDto_ReturnsBadRequest`

## Prohibited Patterns
- No test dependencies between tests
- No real database connections (use in-memory SQLite or mocks)
- No hard-coded values (use meaningful test data)
- No complex test logic (keep simple and focused)
- No missing assertions (every test must verify behavior)


## Complete Examples
For detailed implementation examples of each test type, refer to the following reference implementations:
- Command Handler: `MemberCommandHandlerTests.cs`
- Query Handler: `GetMemberByIdQueryHandlerTests.cs`
- Repository: `MemberRepositoryTests.cs`
- API Endpoint: `MemberEndpointsTests.cs`
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<I{{EntityName}}Repository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<Create{{EntityName}}CommandHandler>> _loggerMock;
    private readonly Create{{EntityName}}CommandHandler _handler;

    public Create{{EntityName}}CommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _repositoryMock = new Mock<I{{EntityName}}Repository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<Create{{EntityName}}CommandHandler>>();
        
        _handler = new Create{{EntityName}}CommandHandler(
            _unitOfWorkMock.Object, _repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }
}
```

Success test pattern:
```csharp
[Fact]
public async Task Handle_ValidCommand_ReturnsSuccess()
{
    // Arrange: create DTOs, entities, setup mocks
    var createDto = new Create{{EntityName}}Dto { PropertyName = "Test" };
    var command = new Create{{EntityName}}Command(createDto);
    var entity = new {{EntityName}} { {{EntityName}}ID = Guid.NewGuid() };
    var entityDto = new {{EntityName}}Dto { {{EntityName}}ID = entity.{{EntityName}}ID };

    _mapperMock.Setup(m => m.Map<{{EntityName}}>(createDto)).Returns(entity);
    _repositoryMock.Setup(r => r.AddAsync(entity)).ReturnsAsync(Result<{{EntityName}}>.Success(entity));
    _mapperMock.Setup(m => m.Map<{{EntityName}}Dto>(entity)).Returns(entityDto);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeTrue();
    result.Value.Should().NotBeNull();
    
    // Verify transaction flow
    _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
    _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    _repositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
}
```

Failure test pattern:
```csharp
[Fact]
public async Task Handle_RepositoryFails_ReturnsFailure()
{
    // Arrange: setup failure scenario
    var createDto = new Create{{EntityName}}Dto { PropertyName = "Test" };
    var command = new Create{{EntityName}}Command(createDto);
    var entity = new {{EntityName}}();

    _mapperMock.Setup(m => m.Map<{{EntityName}}>(createDto)).Returns(entity);
    _repositoryMock.Setup(r => r.AddAsync(entity))
        .ReturnsAsync(Result<{{EntityName}}>.Failure("Database error"));

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeFalse();
    result.Error.Should().Be("Database error");

    // Verify rollback on failure
    _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
    _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
    _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
}
```

## Query Handler Testing
Test structure per handler:
- Mock dependencies: `I{{EntityName}}Repository`, `IMapper`, `ILogger<T>`
- No transaction management (read-only)
- Test success and not-found scenarios

Canonical test class setup:
```csharp
public class Get{{EntityName}}ByIdQueryHandlerTests
{
    private readonly Mock<I{{EntityName}}Repository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<Get{{EntityName}}ByIdQueryHandler>> _loggerMock;
    private readonly Get{{EntityName}}ByIdQueryHandler _handler;

    public Get{{EntityName}}ByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<I{{EntityName}}Repository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<Get{{EntityName}}ByIdQueryHandler>>();
        
        _handler = new Get{{EntityName}}ByIdQueryHandler(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }
}
```

Success test pattern:
```csharp
[Fact]
public async Task Handle_ValidQuery_ReturnsSuccess()
{
    // Arrange
    var id = Guid.NewGuid();
    var query = new Get{{EntityName}}ByIdQuery(id);
    var entity = new {{EntityName}} { {{EntityName}}ID = id, PropertyName = "Test" };
    var entityDto = new {{EntityName}}Dto { {{EntityName}}ID = id, PropertyName = "Test" };

    _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(Result<{{EntityName}}>.Success(entity));
    _mapperMock.Setup(m => m.Map<{{EntityName}}Dto>(entity)).Returns(entityDto);

    // Act
    var result = await _handler.Handle(query, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeTrue();
    result.Value.Should().Be(entityDto);
}
```

## Repository Testing
Use in-memory SQLite for integration testing:
- `SQLiteConnection("DataSource=:memory:")`
- Create table schema matching entity structure
- Setup SQL query mocks via `ISqlQueryLoader`
- Test CRUD operations with real database interactions

Test class setup pattern:
```csharp
public class {{EntityName}}RepositoryTests : IDisposable
{
    private readonly IDbConnection _connection;
    private readonly Mock<IDbConnectionFactory> _mockConnectionFactory;
    private readonly Mock<ISqlQueryLoader> _mockQueryLoader;
    private readonly Mock<ILogger<{{EntityName}}Repository>> _mockLogger;
    private readonly {{EntityName}}Repository _repository;

    public {{EntityName}}RepositoryTests()
    {
        _connection = new SQLiteConnection("DataSource=:memory:");
        _connection.Open();
        
        _mockConnectionFactory = new Mock<IDbConnectionFactory>();
        _mockQueryLoader = new Mock<ISqlQueryLoader>();
        _mockLogger = new Mock<ILogger<{{EntityName}}Repository>>();
        
        _mockConnectionFactory.Setup(x => x.CreateConnection()).Returns(_connection);
        _repository = new {{EntityName}}Repository(_mockConnectionFactory.Object, _mockQueryLoader.Object, _mockLogger.Object);
        
        SetupDatabase();
        SetupSqlQueries();
    }

    private void SetupDatabase()
    {
        // CREATE TABLE with entity schema
        var createTableSql = @"CREATE TABLE {{EntityNamePlural}} ({{EntityName}}ID TEXT PRIMARY KEY, PropertyName TEXT NOT NULL, IsActive INTEGER NOT NULL DEFAULT 1, CreatedDate TEXT NOT NULL)";
        _connection.Execute(createTableSql);
    }

    private void SetupSqlQueries()
    {
        _mockQueryLoader.Setup(x => x.LoadQuery("Get{{EntityName}}ById"))
            .Returns("SELECT * FROM {{EntityNamePlural}} WHERE {{EntityName}}ID = @Id AND IsActive = 1");
        // Setup other queries...
    }

    public void Dispose() => _connection?.Dispose();
}
```

## API Endpoint Testing
Test static endpoint methods with mocked handlers:
- Mock command/query handlers
- Test success and error scenarios
- Verify correct HTTP status codes and response types

Test class setup pattern:
```csharp
public class {{EntityName}}EndpointsTests
{
    private readonly Mock<Get{{EntityName}}ByIdQueryHandler> _mockGetByIdHandler;
    private readonly Mock<Create{{EntityName}}CommandHandler> _mockCreateHandler;

    public {{EntityName}}EndpointsTests()
    {
        _mockGetByIdHandler = new Mock<Get{{EntityName}}ByIdQueryHandler>(Mock.Of<I{{EntityName}}Repository>(), Mock.Of<IMapper>(), Mock.Of<ILogger<Get{{EntityName}}ByIdQueryHandler>>());
        _mockCreateHandler = new Mock<Create{{EntityName}}CommandHandler>(Mock.Of<IUnitOfWork>(), Mock.Of<I{{EntityName}}Repository>(), Mock.Of<IMapper>(), Mock.Of<ILogger<Create{{EntityName}}CommandHandler>>());
    }
}
```

Endpoint test pattern:
```csharp
[Fact]
public async Task Get{{EntityName}}ById_ShouldReturnOk_WhenEntityExists()
{
    // Arrange
    var id = Guid.NewGuid();
    var entity = new {{EntityName}}Dto { {{EntityName}}ID = id, PropertyName = "Test" };
    _mockGetByIdHandler.Setup(x => x.Handle(It.IsAny<Get{{EntityName}}ByIdQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(Result<{{EntityName}}Dto>.Success(entity));

    // Act
    var result = await {{EntityName}}Endpoints.Get{{EntityName}}ById(id, _mockGetByIdHandler.Object, CancellationToken.None);

    // Assert
    var okResult = Assert.IsType<Ok<ApiResponse<{{EntityName}}Dto>>>(result.Result);
    okResult.Value.Success.Should().BeTrue();
    okResult.Value.Data.Should().Be(entity);
}
```

## Code Coverage Requirements
- Aim for minimum 80% code coverage across all projects
- 100% coverage for critical business logic
- Coverage report generated as part of CI pipeline

To generate coverage reports locally:
```bash
dotnet test --collect:"XPlat Code Coverage"
```

View the report in the browser:
```bash
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage-report -open
```

## FluentAssertions Patterns
Result<T> assertions:
```csharp
// Success
result.Should().NotBeNull();
result.IsSuccess.Should().BeTrue();
result.Value.Should().NotBeNull();
result.Value!.PropertyName.Should().Be("Expected Value");

// Failure
result.Should().NotBeNull();
result.IsSuccess.Should().BeFalse();
result.Error.Should().Be("Expected Error Message");
result.Error.Should().Contain("partial error text");
```

Mock verification patterns:
```csharp
// Method called once
_repositoryMock.Verify(r => r.AddAsync(It.IsAny<{{EntityName}}>()), Times.Once);

// Method never called
_unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);

// Transaction flow verification
_unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
_unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
_unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Never);
```

## Test Organization & Naming
Test class naming:
- `{{EntityName}}RepositoryTests`
- `Create{{EntityName}}CommandHandlerTests`
- `Get{{EntityName}}ByIdQueryHandlerTests`
- `{{EntityName}}EndpointsTests`

Test method naming: `MethodName_Condition_ExpectedResult`
- `Handle_ValidCommand_ReturnsSuccess`
- `GetByIdAsync_EntityNotFound_ReturnsFailure`
- `Create{{EntityName}}_InvalidDto_ReturnsBadRequest`

Test structure: Arrange-Act-Assert pattern with clear sections.

## Prohibited Patterns
- No test dependencies between tests
- No real database connections (use in-memory SQLite or mocks)
- No hard-coded values (use meaningful test data)
- No complex test logic (keep simple and focused)
- No missing assertions (every test must verify behavior)

## Integration Points
- Data layer: repository tests with in-memory SQLite
- Logic layer: handler tests with mocked repositories/dependencies
- API layer: endpoint tests with mocked handlers
- Common layer: uses `Result<T>`, `ApiResponse<T>`, shared types in assertions
