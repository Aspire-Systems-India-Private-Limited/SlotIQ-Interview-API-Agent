using SlotIQ.Interview.Common.Enums;

namespace SlotIQ.Interview.Logic.Dtos;

/// <summary>
/// DTO for updating an existing Member (excludes ID, UserName, IsActive, and CreatedDate)
/// UserName is immutable per FR#MAP-2 requirements
/// IsActive should be modified via Deactivate API per FR#MAP-2 requirements
/// </summary>
public class UpdateMemberDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EmailID { get; set; }
    public string? PhoneNumber { get; set; }
    public MemberRoleEnum? RoleID { get; set; }
    public Guid? PracticeID { get; set; }
    public SourceEnum Source { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
}
