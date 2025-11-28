namespace SlotIQ.Interview.Logic.Dtos;

public record MemberLoginResponse
{
    public string Token { get; set; } = string.Empty;
    public MemberDto Member { get; set; } = null!;
}
