namespace SlotIQ.Interview.Logic.Dtos;

public record MemberLoginRequest
{
    public string UserNameOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
