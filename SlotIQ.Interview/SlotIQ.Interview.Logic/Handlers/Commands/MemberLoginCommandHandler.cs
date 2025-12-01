
using BCrypt.Net;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Commands;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Services;
public class MemberLoginCommandHandler
{
    private readonly IMemberRepository _memberRepository;
    private readonly SlotIQ.Interview.Logic.Services.IJwtTokenService _jwtTokenService;

    public MemberLoginCommandHandler(IMemberRepository memberRepository, IJwtTokenService jwtTokenService)
    {
        _memberRepository = memberRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<MemberLoginResponseDto>> HandleAsync(
        MemberLoginCommand command,
        CancellationToken cancellationToken = default)
    {
        // Validate input
            if (string.IsNullOrWhiteSpace(command.UserNameOrEmail))
            {
                return Result<MemberLoginResponseDto>.Failure(ErrorMessages.UserNameRequired);
            }

            if (string.IsNullOrWhiteSpace(command.Password))
            {
                return Result<MemberLoginResponseDto>.Failure("Password is required.");
            }

            // Try to get all members and match username/email manually (since no GetByUserNameAsync/GetByEmailAsync)
            var allMembersResult = await _memberRepository.GetAllAsync();
            if (!allMembersResult.IsSuccess || allMembersResult.Value == null)
            {
                return Result<MemberLoginResponseDto>.Failure(ErrorMessages.MemberNotFound);
            }
            var member = allMembersResult.Value.FirstOrDefault(m =>
                string.Equals(m.UserName, command.UserNameOrEmail, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(m.EmailID, command.UserNameOrEmail, StringComparison.OrdinalIgnoreCase));
            if (member == null)
            {
                return Result<MemberLoginResponseDto>.Failure(ErrorMessages.MemberNotFound);
            }

            if (!BCrypt.Net.BCrypt.Verify(command.Password, member.Password))
            {
                return Result<MemberLoginResponseDto>.Failure("Invalid username/email or password.");
            }

            if (!member.IsActive)
            {
                return Result<MemberLoginResponseDto>.Failure("User account is not active.");
            }

        // Optionally update last login if method exists
        // await _memberRepository.UpdateLastLoginAsync(member.MemberID, DateTime.UtcNow, cancellationToken);

        // Generate JWT token
        var token = _jwtTokenService.GenerateToken(member);

        // Map to DTO
        var memberDto = new MemberDto
        {
            MemberID = member.MemberID,
            UserName = member.UserName,
            Firstname = member.FirstName,
            Lastname = member.LastName,
            EmailID = member.EmailID,
            PhoneNumber = member.PhoneNumber,
            RoleName = member.RoleID,
            PracticeID = member.PracticeID,
            IsActive = member.IsActive,
            CreatedDate = member.CreatedDate,
            ModifiedDate = member.ModifiedDate,
            ModUser = member.ModUser,
            Source = member.Source
        };

        var response = new MemberLoginResponseDto
        {
            Token = token,
            Member = memberDto
        };

        return Result<MemberLoginResponseDto>.Success(response);
    }
}

public class MemberLoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public MemberDto Member { get; set; } = default!;
}
