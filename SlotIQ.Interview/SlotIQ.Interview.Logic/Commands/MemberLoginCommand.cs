using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.Logic.Commands;

public record MemberLoginCommand
{
    public string UserNameOrEmail { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
