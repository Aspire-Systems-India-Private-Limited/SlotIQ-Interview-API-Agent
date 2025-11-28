using BCrypt.Net;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Commands;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Services;

namespace SlotIQ.Interview.Logic.Handlers.Commands;

public class MemberLoginCommandHandler
{
    private readonly IMemberRepository _memberRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public MemberLoginCommandHandler(IMemberRepository memberRepository, IJwtTokenService jwtTokenService)
    {
        _memberRepository = memberRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<(bool Success, string? Token, MemberDto? Member, string? ErrorMessage)> HandleAsync(
        MemberLoginCommand command, 
        CancellationToken cancellationToken = default)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(command.UserNameOrEmail))
        {
            return (false, null, null, "Username or email is required.");
        }

        if (string.IsNullOrWhiteSpace(command.Password))
        {
            return (false, null, null, "Password field is required and cannot be empty.");
        }

        // Get member by username or email
        var member = await _memberRepository.GetByUserNameOrEmailAsync(command.UserNameOrEmail, cancellationToken);

        if (member == null)
        {
            return (false, null, null, "User not found.");
        }

        // Verify password
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(command.Password, member.PasswordHash);
        if (!isPasswordValid)
        {
            return (false, null, null, "Invalid username/email or password.");
        }

        // Check if user is active
        if (!member.IsActive)
        {
            return (false, null, null, "User account is not active.");
        }

        // Update last login
        await _memberRepository.UpdateLastLoginAsync(member.MemberID, DateTime.UtcNow, cancellationToken);

        // Generate JWT token
        var token = _jwtTokenService.GenerateToken(member);

        // Map to DTO
        var memberDto = new MemberDto
        {
            MemberID = member.MemberID,
            UserName = member.UserName,
            Firstname = member.Firstname,
            Lastname = member.Lastname,
            EmailID = member.EmailID,
            PhoneNumber = member.PhoneNumber,
            RoleName = member.RoleName,
            PracticeID = member.PracticeID,
            IsActive = member.IsActive,
            CreatedDate = member.CreatedDate,
            ModifiedDate = member.ModifiedDate,
            ModUser = member.ModUser,
            Source = member.Source
        };

        return (true, token, memberDto, null);
    }
}
