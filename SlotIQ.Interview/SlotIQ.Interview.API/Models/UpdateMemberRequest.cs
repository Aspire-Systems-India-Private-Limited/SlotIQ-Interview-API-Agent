using SlotIQ.Interview.Common.Enums;

namespace SlotIQ.Interview.API.Models;

/// <summary>
/// Request model for updating an existing member
/// </summary>
public class UpdateMemberRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EmailAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public MemberRoleEnum? RoleName { get; set; }
    public Guid? PracticeID { get; set; }
    public SourceEnum Source { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
}
