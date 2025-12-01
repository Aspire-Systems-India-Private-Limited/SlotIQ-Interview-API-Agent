using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data;
using SlotIQ.Interview.Data.Entities;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Commands;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.Logic.Handlers.Commands;

/// <summary>
/// Handler for UpdateMemberCommand
/// </summary>
public class UpdateMemberCommandHandler : ICommandHandler<UpdateMemberCommand, Result<MemberDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateMemberCommandHandler> _logger;
    private readonly IValidator<UpdateMemberDto> _validator;

    public UpdateMemberCommandHandler(
        IUnitOfWork unitOfWork,
        IMemberRepository memberRepository,
        IMapper mapper,
        ILogger<UpdateMemberCommandHandler> logger,
        IValidator<UpdateMemberDto> validator)
    {
        _unitOfWork = unitOfWork;
        _memberRepository = memberRepository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<MemberDto>> Handle(UpdateMemberCommand command, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating member with ID {MemberID}", command.MemberID);

            // Validate the DTO
            var validationResult = await _validator.ValidateAsync(command.Dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed for member update: {Errors}", errors);
                return Result<MemberDto>.Failure(errors);
            }

            // Check if member exists
            var existingMemberResult = await _memberRepository.GetByIdAsync(command.MemberID);
            if (!existingMemberResult.IsSuccess)
            {
                _logger.LogWarning("Member with ID {MemberID} not found", command.MemberID);
                return Result<MemberDto>.Failure(ErrorMessages.MemberNotFound);
            }

            var existingMember = existingMemberResult.Value!;

            // Check for duplicate email if email is being updated
            if (!string.IsNullOrEmpty(command.Dto.EmailID) && 
                !existingMember.EmailID.Equals(command.Dto.EmailID, StringComparison.OrdinalIgnoreCase))
            {
                if (await _memberRepository.EmailExistsAsync(command.Dto.EmailID))
                {
                    _logger.LogWarning("Email {EmailID} already exists", command.Dto.EmailID);
                    return Result<MemberDto>.Failure(ErrorMessages.DuplicateEmailAddress);
                }
            }

            // Check for duplicate phone number if phone number is being updated
            if (!string.IsNullOrEmpty(command.Dto.PhoneNumber) && 
                !existingMember.PhoneNumber.Equals(command.Dto.PhoneNumber, StringComparison.OrdinalIgnoreCase))
            {
                if (await _memberRepository.PhoneNumberExistsAsync(command.Dto.PhoneNumber))
                {
                    _logger.LogWarning("Phone number {PhoneNumber} already exists", command.Dto.PhoneNumber);
                    return Result<MemberDto>.Failure(ErrorMessages.DuplicatePhoneNumber);
                }
            }

            // Apply updates only to fields that are provided (not null)
            if (!string.IsNullOrEmpty(command.Dto.FirstName))
                existingMember.FirstName = command.Dto.FirstName;

            if (!string.IsNullOrEmpty(command.Dto.LastName))
                existingMember.LastName = command.Dto.LastName;

            if (!string.IsNullOrEmpty(command.Dto.EmailID))
                existingMember.EmailID = command.Dto.EmailID;

            if (!string.IsNullOrEmpty(command.Dto.PhoneNumber))
                existingMember.PhoneNumber = command.Dto.PhoneNumber;

            if (command.Dto.RoleID.HasValue)
                existingMember.RoleID = command.Dto.RoleID.Value;

            if (command.Dto.PracticeID.HasValue)
                existingMember.PracticeID = command.Dto.PracticeID.Value;

            existingMember.Source = command.Dto.Source;
            existingMember.ModifiedDate = DateTime.UtcNow;
            existingMember.ModifiedBy = command.Dto.ModifiedBy;

            // Update member in database
            var result = await _memberRepository.UpdateAsync(existingMember);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to update member: {Error}", result.Error);
                return Result<MemberDto>.Failure(result.Error ?? ErrorMessages.MemberUpdateFailure);
            }

            // Map entity back to DTO
            var memberDto = _mapper.Map<MemberDto>(result.Value);

            _logger.LogInformation("Member updated successfully with ID {MemberID}", memberDto.MemberID);
            return Result<MemberDto>.Success(memberDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating member with ID {MemberID}", command.MemberID);
            return Result<MemberDto>.Failure(ErrorMessages.SystemError);
        }
    }
}
