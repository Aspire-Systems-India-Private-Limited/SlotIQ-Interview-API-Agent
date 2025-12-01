using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.Logic.Commands;

/// <summary>
/// Command for updating an existing Member
/// </summary>
public record UpdateMemberCommand(Guid MemberID, UpdateMemberDto Dto) : ICommand<Result<MemberDto>>;
