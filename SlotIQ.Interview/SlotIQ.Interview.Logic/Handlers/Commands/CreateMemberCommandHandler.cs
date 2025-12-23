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
using System.Security.Cryptography;
using System.Text;

namespace SlotIQ.Interview.Logic.Handlers.Commands;

/// <summary>
/// Handler for CreateMemberCommand
/// </summary>
public class CreateMemberCommandHandler : ICommandHandler<CreateMemberCommand, Result<MemberDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateMemberCommandHandler> _logger;
    private readonly IValidator<CreateMemberDto> _validator;

    public CreateMemberCommandHandler(
        IUnitOfWork unitOfWork,
        IMemberRepository memberRepository,
        IMapper mapper,
        ILogger<CreateMemberCommandHandler> logger,
        IValidator<CreateMemberDto> validator)
    {
        _unitOfWork = unitOfWork;
        _memberRepository = memberRepository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<MemberDto>> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating member with username {UserName}", command.Dto.UserName);

            // Validate the DTO
            var validationResult = await _validator.ValidateAsync(command.Dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validation failed for member creation: {Errors}", errors);
                return Result<MemberDto>.Failure(errors);
            }

            // Map DTO to Entity
            var member = _mapper.Map<Member>(command.Dto);
            member.MemberID = Guid.NewGuid();
            member.Password = GeneratePassword();
            member.IsActive = true;
            member.CreatedDate = DateTime.UtcNow;
            member.ModifiedDate = DateTime.UtcNow;
            member.ModifiedBy = command.Dto.CreatedBy;

            // Add member to database
            var result = await _memberRepository.AddAsync(member);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to create member: {Error}", result.Error);
                return Result<MemberDto>.Failure(result.Error ?? ErrorMessages.UserOnboardFailure);
            }

            // Map entity back to DTO
            var memberDto = _mapper.Map<MemberDto>(result.Value);

            _logger.LogInformation("Member created successfully with ID {MemberID}", memberDto.MemberID);
            return Result<MemberDto>.Success(memberDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating member with username {UserName}", command.Dto.UserName);
            return Result<MemberDto>.Failure(ErrorMessages.SystemError);
        }
    }

    private static string GeneratePassword()
    {
        const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789@#$-_";
        var random = new byte[12];
        RandomNumberGenerator.Fill(random);
        
        var password = new StringBuilder(12);
        for (int i = 0; i < 12; i++)
        {
            password.Append(validChars[random[i] % validChars.Length]);
        }
        
        return password.ToString();
    }
}
