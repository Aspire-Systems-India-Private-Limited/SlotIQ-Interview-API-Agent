---
applyTo: SlotIQ.Interview.Logic/**/*.cs
description: Logic layer patterns for CQRS commands, queries, handlers, DTOs, and business logic using custom CQRS implementation
---
## Purpose
Defines custom CQRS patterns for business logic. Contains command/query handlers, DTOs, and AutoMapper configurations. No MediatR - uses record types and generic interfaces for commands/queries with direct handler injection.

## Variables
- `{{EntityName}}`: singular entity (e.g., AvailabilityPlan)
- `{{EntityNamePlural}}`: plural entity (e.g., AvailabilityPlans)
- `{{ProjectPrefix}}`: root namespace (e.g., SlotIQ.Interview)

## Core Principles
- Custom CQRS implementation. No MediatR.
- Record types for immutable commands/queries.
- Generic interfaces specify return types.
- All handlers return `Result<T>` or `PaginatedResult<T>`.
- Transaction management via `IUnitOfWork` for commands only.
- AutoMapper for entity-DTO mapping.
- File-scoped namespaces in all C# files.
- Primary keys use `{{EntityName}}ID` (ID suffix, not Id).

## CQRS Interfaces
Base interfaces for custom CQRS:
```csharp
public interface ICommand<T> where T : class { }
public interface IQuery<T> where T : class { }

public interface ICommandHandler<TCommand, TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface IQueryHandler<TQuery, TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
}
```

## DTO Structure Patterns
Three DTO types per entity:

**Main DTO** (`{{EntityName}}Dto`):
- Include `{{EntityName}}ID` (Guid primary key)
- All business properties
- Audit fields: `IsActive`, `CreatedDate`, `ModifiedDate`, `ModUser`, `Source`

**Create DTO** (`Create{{EntityName}}Dto`):
- Business properties only
- Exclude ID and audit fields

**Update DTO** (`Update{{EntityName}}Dto`):
- Business properties + `IsActive` for soft delete
- Exclude ID, `CreatedDate`, `ModifiedDate`

## Command Patterns
Record types implementing `ICommand<Result<T>>`:
```csharp
public record Create{{EntityName}}Command(Create{{EntityName}}Dto Dto) : ICommand<Result<{{EntityName}}Dto>>;
public record Update{{EntityName}}Command(Guid Id, Update{{EntityName}}Dto Dto) : ICommand<Result<{{EntityName}}Dto>>;
public record Delete{{EntityName}}Command(Guid Id) : ICommand<Result<bool>>;
```

## Query Patterns
Record types implementing `IQuery<Result<T>>`:
```csharp
public record Get{{EntityName}}ByIdQuery(Guid Id) : IQuery<Result<{{EntityName}}Dto>>;
public record GetAll{{EntityName}}sQuery() : IQuery<Result<IEnumerable<{{EntityName}}Dto>>>;
public record GetPaged{{EntityName}}sQuery(int PageNumber, int PageSize) : IQuery<Result<PaginatedResult<{{EntityName}}Dto>>>;
```

## Command Handler Pattern
Structure and dependencies:
- Inject: `IUnitOfWork`, `I{{EntityName}}Repository`, `IMapper`, `ILogger<T>`
- Validation : 
    - ALL validation logic must be implemented in FluentValidation validators, NOT in command handlers
    - All business rule validation is handled by FluentValidation validators
    - Command handlers should ONLY call `_validator.ValidateAsync()` and handle the result
    - Validators must implement ALL validations defined in FR#MAP requirement documents under "API Level Business Rules"
    - Database-dependent validations (duplicates, resource existence) must be implemented in validators using `MustAsync`
    - Duplicate check calls should not be in command handlers
    - Remove resource existence checks from command handlers - these belong in validators
    - Error codes and messages must match exactly those specified in FR#MAP requirement documents
    - Use constants for all validation error messages
- Transaction management: `BeginTransactionAsync()`, `CommitAsync()`, `RollbackAsync()`- Skip transaction scope if its a single Insert/Update call(s).
	- Start transaction scope just before Insert/Update/Delete call(s)
	- Dont keep Get calls within transaction scope.
- Error handling: rollback on failures, use `ErrorMessages` constants
- Mapping: TCommand ↔ DTO via AutoMapper
    -consistently implement enum↔ID mapping in MappingProfiles, 

Canonical example (Create):
```csharp
public class Create{{EntityName}}CommandHandler : ICommandHandler<Create{{EntityName}}Command, Result<{{EntityName}}Dto>>
{
    // Constructor injection: IUnitOfWork, I{{EntityName}}Repository, IMapper, ILogger
    
    public async Task<Result<{{EntityName}}Dto>> Handle(Create{{EntityName}}Command command, CancellationToken ct)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var entity = _mapper.Map<{{EntityName}}>(command.Dto);
            var result = await _repository.AddAsync(entity);
            
            if (!result.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return Result<{{EntityName}}Dto>.Failure(result.Error ?? ErrorMessages.OperationFailed);
            }
            
            await _unitOfWork.CommitAsync();
            return Result<{{EntityName}}Dto>.Success(_mapper.Map<{{EntityName}}Dto>(result.Value));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating {{EntityName}}");
            await _unitOfWork.RollbackAsync();
            return Result<{{EntityName}}Dto>.Failure(ErrorMessages.OperationFailed);
        }
    }
}
```

Other patterns:
- Update: similar to Create, use `UpdateAsync()`, check for not-found errors
- Delete: use `DeleteAsync()`, return `Result<bool>`

## Query Handler Pattern
Structure and dependencies:
- Inject: `I{{EntityName}}Repository`, `IMapper`, `ILogger<T>`
- No transaction management (read-only)
- Direct repository access
- Error handling: use `ErrorMessages` constants

Canonical example (GetById):
```csharp
public class Get{{EntityName}}ByIdQueryHandler : IQueryHandler<Get{{EntityName}}ByIdQuery, Result<{{EntityName}}Dto>>
{
    // Constructor injection: I{{EntityName}}Repository, IMapper, ILogger
    
    public async Task<Result<{{EntityName}}Dto>> Handle(Get{{EntityName}}ByIdQuery query, CancellationToken ct)
    {
        try
        {
            var result = await _repository.GetByIdAsync(query.Id);
            if (!result.IsSuccess)
                return Result<{{EntityName}}Dto>.Failure(result.Error ?? ErrorMessages.{{EntityName}}NotFound);
            
            return Result<{{EntityName}}Dto>.Success(_mapper.Map<{{EntityName}}Dto>(result.Value));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving {{EntityName}} with ID {Id}", query.Id);
            return Result<{{EntityName}}Dto>.Failure(ErrorMessages.DatabaseError);
        }
    }
}
```

## Pagination Implementation
When implementing paged queries:

```csharp
public class GetPaged{{EntityName}}sQueryHandler : IQueryHandler<GetPaged{{EntityName}}sQuery, Result<PaginatedResult<{{EntityName}}Dto>>>
{
    // Constructor injection: I{{EntityName}}Repository, IMapper, ILogger
    
    public async Task<Result<PaginatedResult<{{EntityName}}Dto>>> Handle(GetPaged{{EntityName}}sQuery query, CancellationToken ct)
    {
        try
        {
            // Validate pagination parameters
            if (query.PageNumber < 1)
                return Result<PaginatedResult<{{EntityName}}Dto>>.Failure(ErrorMessages.InvalidPageNumber);
            
            if (query.PageSize < 1 || query.PageSize > 50)
                return Result<PaginatedResult<{{EntityName}}Dto>>.Failure(ErrorMessages.InvalidPageSize);
                
            var result = await _repository.GetPagedAsync(query.PageNumber, query.PageSize);
            
            if (!result.IsSuccess)
                return Result<PaginatedResult<{{EntityName}}Dto>>.Failure(result.Error);
                
            // Map each entity in the page to DTO
            var dtos = result.Items.Select(entity => _mapper.Map<{{EntityName}}Dto>(entity)).ToList();
            
            // Create a new paginated result with the mapped DTOs
            var pagedResult = new PaginatedResult<{{EntityName}}Dto>
            {
                Items = dtos,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                IsSuccess = true
            };
            
            return Result<PaginatedResult<{{EntityName}}Dto>>.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paged {{EntityNamePlural}}");
            return Result<PaginatedResult<{{EntityName}}Dto>>.Failure(ErrorMessages.DatabaseError);
        }
    }
}
```

## DTO Validation with FluentValidation
Implement validation for each DTO:

```csharp
public class Create{{EntityName}}DtoValidator : AbstractValidator<Create{{EntityName}}Dto>
{
    public Create{{EntityName}}DtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ErrorMessages.Required("Name"))
            .MaximumLength(100).WithMessage(ErrorMessages.MaxLength("Name", 100));
            
        // Add rules for other properties
        
        // Example for email validation
        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage(ErrorMessages.InvalidEmail);
    }
}
```

Register validators in dependency injection:
```csharp
// In Program.cs or Startup.cs
services.AddFluentValidationAutoValidation();
services.AddValidatorsFromAssemblyContaining<Create{{EntityName}}DtoValidator>();
```

## AutoMapper Configuration
Mapping profile per entity (`{{EntityName}}MappingProfile : Profile`):

Required mappings:
- Entity ↔ DTO (bidirectional)
- Create DTO → Entity: set `{{EntityName}}ID = Guid.NewGuid()`, `CreatedDate = DateTime.UtcNow`, `IsActive = true`
- Update DTO → Entity: ignore `{{EntityName}}ID` and `CreatedDate`, set `ModifiedDate = DateTime.UtcNow`

Example:
```csharp
public class {{EntityName}}MappingProfile : Profile
{
    public {{EntityName}}MappingProfile()
    {
        CreateMap<{{EntityName}}, {{EntityName}}Dto>();
        CreateMap<{{EntityName}}Dto, {{EntityName}}>();
        
        CreateMap<Create{{EntityName}}Dto, {{EntityName}}>()
            .ForMember(d => d.{{EntityName}}ID, o => o.MapFrom(_ => Guid.NewGuid()))
            .ForMember(d => d.CreatedDate, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.IsActive, o => o.MapFrom(_ => true));
            .ForMember(d => d.RoleName, o => o.MapFrom(s => (MemberRole)s.RoleID))
            .ForMember(d => d.Source, o => o.MapFrom(s => (Source)Enum.Parse(typeof(Source), s.Source ?? "1")))
        
        CreateMap<Update{{EntityName}}Dto, {{EntityName}}>()
            .ForMember(d => d.{{EntityName}}ID, o => o.Ignore())
            .ForMember(d => d.CreatedDate, o => o.Ignore())
            .ForMember(d => d.ModifiedDate, o => o.MapFrom(_ => DateTime.UtcNow));

      
          CreateMap<CreateMemberCommand, Member>()
                .ForMember(d => d.MemberID, o => o.Ignore())
                .ForMember(d => d.RoleID,   o => o.MapFrom(s => (int)s.RoleName))
                .ForMember(d => d.Source,   o => o.MapFrom(s => ((int)s.Source).ToString()));

                CreateMap<CreateMemberDto, Member>()
                    .ForMember(d => d.MemberID, o => o.Ignore())
                    .ForMember(d => d.RoleID,  o => o.MapFrom(s => (int)s.RoleName))
                    .ForMember(d => d.Source,  o => o.MapFrom(s => ((int)s.Source).ToString()));
           
    }
}
```

## Error Handling Strategy
Follow these patterns for consistent error handling:

1. **Entity Not Found**: Use `ErrorMessages.{{EntityName}}NotFound`
2. **Validation Errors**: Return specific validation error messages
3. **Database Errors**: Catch exceptions, log with context, return `ErrorMessages.DatabaseError`
4. **Business Rule Violations**: Use specific error messages for business rules
5. **Authorization Failures**: Return appropriate authorization error messages

## Naming Conventions
- Commands: `Create{{EntityName}}Command`, `Update{{EntityName}}Command`, `Delete{{EntityName}}Command`
- Queries: `Get{{EntityName}}ByIdQuery`, `GetAll{{EntityName}}sQuery`, `GetPaged{{EntityName}}sQuery`
- Handlers: `Create{{EntityName}}CommandHandler`, `Get{{EntityName}}ByIdQueryHandler`
- DTOs: `{{EntityName}}Dto`, `Create{{EntityName}}Dto`, `Update{{EntityName}}Dto`
- Mapping: `{{EntityName}}MappingProfile`
- Primary keys: `{{EntityName}}ID` (not `{{EntityName}}Id`)

## Dependencies
- AutoMapper v12.0.1 + Extensions v12.0.1
- FluentValidation v11.9.2 + DI Extensions v11.9.2
- Microsoft.Extensions.Logging.Abstractions v9.0.0
- Microsoft.Extensions.DependencyInjection v9.0.0

## Prohibited Patterns
- No MediatR. Use custom CQRS.
- No class commands/queries. Use record types.
- No direct entity returns. Always return DTOs.
- No transactions in queries. Only in command handlers.
- No magic strings. Use `ErrorMessages` constants.

## Integration Points
- Data layer: repository interfaces and `IUnitOfWork`
- Common layer: `Result<T>`, `PaginatedResult<T>`, `ErrorMessages`
- API layer: handlers injected directly in endpoints
