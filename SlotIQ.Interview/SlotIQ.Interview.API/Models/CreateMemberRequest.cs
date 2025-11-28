using SlotIQ.Interview.Common.Enums;

namespace SlotIQ.Interview.API.Models;

/// <summary>
/// Request model for creating a new member
/// </summary>
public class CreateMemberRequest
{
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailID { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public MemberRoleEnum RoleID { get; set; }
    public Guid PracticeID { get; set; }
    public SourceEnum Source { get; set; }
}
