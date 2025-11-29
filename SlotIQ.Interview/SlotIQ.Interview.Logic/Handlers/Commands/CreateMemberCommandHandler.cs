using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data.Entities;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Commands;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.Logic.Handlers.Commands;

public class CreateMemberCommandHandler : ICommandHandler<CreateMemberCommand, Result<MemberDto>>
{
    private readonly IMemberRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateMemberCommandHandler> _logger;
    private readonly IValidator<CreateMemberDto> _validator;

    public CreateMemberCommandHandler(
        IMemberRepository repository,
        IMapper mapper,
        ILogger<CreateMemberCommandHandler> logger,
        IValidator<CreateMemberDto> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<MemberDto>> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
    {
        try
        {
            // Validate the DTO
            var validationResult = await _validator.ValidateAsync(command.Dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed for CreateMemberCommand: {Errors}", errors);
                return Result<MemberDto>.Failure(errors);
            }

            // Map DTO to Entity
            var entity = _mapper.Map<Member>(command.Dto);
            entity.MemberID = Guid.NewGuid();
            entity.CreatedDate = DateTime.UtcNow;
            entity.ModifiedDate = DateTime.UtcNow;
            entity.CreatedBy = command.Dto.UpdatedBy;
            entity.ModifiedBy = command.Dto.UpdatedBy;

            // Insert member
            var result = await _repository.AddAsync(entity);
            
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to create member: {Error}", result.Error);
                return Result<MemberDto>.Failure(result.Error ?? ErrorMessages.OperationFailed);
            }

            var memberDto = _mapper.Map<MemberDto>(result.Value);
            _logger.LogInformation("Successfully created member with ID {MemberID}", memberDto.MemberID);
            
            return Result<MemberDto>.Success(memberDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating member");
            return Result<MemberDto>.Failure(ErrorMessages.OperationFailed);
        }
    }
}
