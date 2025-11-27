using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.Logic.Commands;

/// <summary>
/// Command for creating a new Member
/// </summary>
public record CreateMemberCommand(CreateMemberDto Dto) : ICommand<Result<MemberDto>>;
