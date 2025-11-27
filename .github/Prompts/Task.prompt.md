---
applyTo: /**/*.cs
description: Task implementation workflow and patterns for converting user stories to code following Clean Architecture with CQRS
---

# Task Implementation Workflow - SlotIQ Interview Solution

## Variable Resolution
Use standardized variables for consistent implementation across all layers:

| Variable               | Usage                                       | Example                    |
|------------------------|---------------------------------------------|----------------------------|
| `{{EntityName}}`       | Replace with actual entity name (singular)  | Member, AvailabilityPlan, AvailabilityRePlanRequest |
| `{{EntityNamePlural}}` | Replace with plural form                    | Members, AvailabilityPlans, AvailabilityRePlanRequests |
| `{{ProjectPrefix}}`    | Replace with solution namespace prefix      | SlotIQ.Interview |### Inputs 
| `{{SchemaJson}}`       | JSON schema defining entity properties      | Domain layer specific      |

## Pre-Implementation Analysis

### User Story Decomposition Checklist
- [ ] **Operation Classification**: CRUD operation type or custom business operation
- [ ] **Business Rules**: Validation requirements, constraints, and business logic
- [ ] **Data Schema**: Required properties, types, relationships, and constraints
- [ ] **Primary Entity**: All Entity, DTO objects to be created/modified
- [ ] **Response Requirements**: DTOs needed for different response scenarios
- [ ] **Integration Dependencies**: External services, repositories, or third-party systems
- [ ] **Performance Considerations**: Pagination, caching, or query optimization needs

### Schema Analysis for Entity-Related Stories
- [ ] Identify required properties and their C# types
- [ ] Determine nullable vs required properties (use `string?` for optional)
- [ ] Map relationships to other entities
- [ ] Define validation constraints
- [ ] Consider display/formatting requirements for DTOs

## Implementation Workflow

### Phase 1: Common Layer
**Reference**: [backend/common-layer.mdc](./../Instructions/common.instructions.md)

**Tasks:**
- [ ] Create/update enums with JsonStringEnumConverter
- [ ] Add error message constants to ErrorMessages class
- [ ] Implement Result<T> patterns if needed
- [ ] Add shared models and types

### Phase 2: Data Layer
**Reference**: [backend/data-layer.md](./../Instructions/data.Instructions.md)

**Tasks:**
- [ ] Create entity with `Guid {{EntityName}}ID` primary key
- [ ] Inherit from BaseEntity for audit fields
- [ ] Create repository interface in `Contracts/` folder
- [ ] Implement repository with Dapper and external SQL files
- [ ] Create SQL query files in `Sql/` folder
- [ ] Update UnitOfWork if needed

**Quality Gates:**
- [ ] Primary key follows `{{EntityName}}ID` pattern (not `{{EntityName}}Id`)
- [ ] All properties from schema are mapped
- [ ] Repository interfaces match implementation
- [ ] SQL query files use parameterized queries

### Phase 3: Logic Layer
**Reference**: [backend/logic-layer.mdc](./../Instructions/logic.instructions.md)

**Tasks:**
- [ ] Create DTOs (Main, Create, Update)
- [ ] Create commands as record types (Create, Update, Delete)
- [ ] Create queries as record types (GetById, GetAll, GetPaged)
- [ ] Implement command handlers with IUnitOfWork
- [ ] Implement query handlers (no transactions)
- [ ] Create AutoMapper profile
- [ ] Add FluentValidation rules

**Quality Gates:**
- [ ] Commands/queries are record types implementing ICommand<T>/IQuery<T>
- [ ] Handlers use custom CQRS interfaces (no MediatR)
- [ ] AutoMapper profiles handle all properties
- [ ] Validation rules cover business requirements

### Phase 4: API Layer
**Reference**: [backend/api-layer.mdc](./../Instructions/api.instructions.md)

**Tasks:**
- [ ] Create static endpoint class `{{EntityName}}Endpoints`
- [ ] Implement CRUD endpoints with static methods
- [ ] Add OpenAPI documentation attributes
- [ ] Use TypedResults for HTTP responses
- [ ] Wrap responses in ApiResponse<T>
- [ ] Register endpoints in Program.cs

**Quality Gates:**
- [ ] All endpoints use static methods (no controllers)
- [ ] Route constraints use `{id:guid}` pattern
- [ ] Handlers injected directly (no MediatR)
- [ ] Error responses use ApiResponse<T> format

### Phase 5: Integration Layer
**Reference**: [backend/integration-layer.mdc](./../Instructions/integration.instructions.md)

**Tasks:**
- [ ] Create service interfaces for external dependencies
- [ ] Implement services for third-party integrations
- [ ] Create adapters for external API communication
- [ ] Implement email services for notifications
- [ ] Create password hashing and security services
- [ ] Register integration services in DI container

**Quality Gates:**
- [ ] Services use dependency injection
- [ ] Integration services have proper error handling
- [ ] External API calls use resilience patterns
- [ ] Sensitive information is properly secured
- [ ] Services follow interface segregation principle

### Phase 6: Testing Layer
**Reference**: [backend/unittest-layer.mdc](./../Instructions/unittest.instructions.md)

**Tasks:**
- [ ] Create repository tests with in-memory SQLite
- [ ] Create command handler tests with mocked dependencies
- [ ] Create query handler tests with test data
- [ ] Create API endpoint tests with mocked handlers
- [ ] Add integration test scenarios
- [ ] Create tests for integration services with mocked external dependencies

**Quality Gates:**
- [ ] Repository tests use in-memory SQLite
- [ ] Handler tests verify IUnitOfWork interactions
- [ ] Endpoint tests assert on TypedResults
- [ ] Tests cover success and failure scenarios
- [ ] Integration services tests verify external dependency interactions

## Final Validation Checklist

### Integration Testing
- [ ] Full CRUD operations work end-to-end
- [ ] Error handling works across all layers
- [ ] Logging provides adequate information
- [ ] Performance meets acceptable standards

### Code Quality
- [ ] All naming conventions followed consistently
- [ ] File-scoped namespaces used in all C# files
- [ ] Proper separation of concerns maintained
- [ ] No code duplication across layers

### Deployment Readiness
- [ ] All dependencies registered in DI container
- [ ] Configuration settings are correct
- [ ] Database schema aligns with entity properties
- [ ] API documentation is complete

## Critical Implementation Rules

### Must Follow
1. **Primary Keys**: Always use `{{EntityName}}ID` suffix (not `{{EntityName}}Id`)
2. **No MediatR**: Use custom CQRS implementation only
3. **Minimal APIs**: Static endpoint methods, no controllers
4. **External SQL**: Store all SQL in .sql files, no inline SQL
5. **Result Pattern**: All operations return Result<T> or PaginatedResult<T>
6. **File Scoped Namespaces**: Required for all C# files

### Prohibited Patterns
- ❌ Using `{{EntityName}}Id` for primary keys
- ❌ MediatR or any external CQRS library
- ❌ Controller classes
- ❌ Inline SQL strings
- ❌ Direct exceptions for business logic errors
- ❌ Hard deletes (use soft delete with IsActive flag)


## Quick Start Template

When implementing a new entity, follow this sequence:
1. **Analyze** user story and decompose requirements
2. **Design** entity schema and relationships
3. **Implement** in layer order: Common → Data → Logic → API → Integration → Tests
4. **Validate** each phase before proceeding
5. **Test** end-to-end functionality
6. **Document** any deviations or special considerations