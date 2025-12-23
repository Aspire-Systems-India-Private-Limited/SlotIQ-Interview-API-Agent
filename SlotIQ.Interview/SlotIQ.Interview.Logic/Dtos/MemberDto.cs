using SlotIQ.Interview.Common.Enums;

namespace SlotIQ.Interview.Logic.Dtos;

public class MemberDto
{
    public Guid MemberID { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailID { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public MemberRole RoleName { get; set; }
    public Guid PracticeID { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string ModUser { get; set; } = string.Empty;
    public Source Source { get; set; }
}
