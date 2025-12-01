using SlotIQ.Interview.Common.Enums;

namespace SlotIQ.Interview.Logic.Dtos;

/// <summary>
/// DTO for updating an existing Member (excludes ID and CreatedDate)
/// </summary>
public class UpdateMemberDto
{
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailID { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public MemberRoleEnum RoleID { get; set; }
    public Guid PracticeID { get; set; }
    public bool IsActive { get; set; }
    public SourceEnum Source { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
}
