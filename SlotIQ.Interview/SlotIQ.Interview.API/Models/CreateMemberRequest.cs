using SlotIQ.Interview.Common.Enums;

namespace SlotIQ.Interview.API.Models;

public class CreateMemberRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public MemberRole RoleName { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public Guid PracticeID { get; set; }
    public bool IsActive { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public Source Source { get; set; }
}
