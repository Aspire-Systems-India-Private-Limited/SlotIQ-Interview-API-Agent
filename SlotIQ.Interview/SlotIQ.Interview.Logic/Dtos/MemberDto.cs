using SlotIQ.Interview.Common.Enums;

namespace SlotIQ.Interview.Logic.Dtos;

/// <summary>
/// Main DTO for Member entity with all properties including audit fields
/// </summary>
public class MemberDto
{
    public Guid MemberID { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailID { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public MemberRoleEnum RoleID { get; set; }
    public Guid PracticeID { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string ModifiedBy { get; set; } = string.Empty;
    public SourceEnum Source { get; set; }
}
