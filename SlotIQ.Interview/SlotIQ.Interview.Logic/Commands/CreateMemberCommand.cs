using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.Logic.Commands;

public record CreateMemberCommand(CreateMemberDto Dto) : ICommand<Result<MemberDto>>;
