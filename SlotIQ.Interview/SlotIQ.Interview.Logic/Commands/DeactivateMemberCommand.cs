using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Common.Models;

namespace SlotIQ.Interview.Logic.Commands;

/// <summary>
/// Command for deactivating a Member (soft delete)
/// </summary>
public record DeactivateMemberCommand(Guid MemberID, string ModifiedBy, SourceEnum Source) : ICommand<Result<string>>;
