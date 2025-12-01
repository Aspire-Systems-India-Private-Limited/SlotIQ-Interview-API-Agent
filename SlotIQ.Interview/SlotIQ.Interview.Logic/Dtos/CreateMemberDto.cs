using SlotIQ.Interview.Common.Enums;

namespace SlotIQ.Interview.Logic.Dtos;

/// <summary>
/// DTO for creating a new Member (excludes ID and audit fields)
/// </summary>
public class CreateMemberDto
{
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailID { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public MemberRoleEnum RoleID { get; set; }
    public Guid PracticeID { get; set; }
    public SourceEnum Source { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}
