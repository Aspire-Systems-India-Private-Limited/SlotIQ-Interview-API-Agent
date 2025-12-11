using Microsoft.Extensions.Logging;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Commands;

namespace SlotIQ.Interview.Logic.Handlers.Commands;

/// <summary>
/// Handler for DeactivateMemberCommand
/// </summary>
public class DeactivateMemberCommandHandler : ICommandHandler<DeactivateMemberCommand, Result<string>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<DeactivateMemberCommandHandler> _logger;

    public DeactivateMemberCommandHandler(
        IMemberRepository memberRepository,
        ILogger<DeactivateMemberCommandHandler> logger)
    {
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(DeactivateMemberCommand command, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Deactivating member with ID {MemberID}", command.MemberID);

            // Deactivate member in database
            var result = await _memberRepository.DeactivateMemberAsync(
                command.MemberID,
                command.ModifiedBy,
                command.Source);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to deactivate member: {Error}", result.Error);
                return Result<string>.Failure(result.Error ?? ErrorMessages.SystemError);
            }

            _logger.LogInformation("Member deactivated successfully with ID {MemberID}", command.MemberID);
            return Result<string>.Success(result.Value!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating member with ID {MemberID}", command.MemberID);
            return Result<string>.Failure(ErrorMessages.SystemError);
        }
    }
}
