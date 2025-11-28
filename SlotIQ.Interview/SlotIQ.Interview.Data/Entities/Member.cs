using SlotIQ.Interview.Common.Enums;

namespace SlotIQ.Interview.Data.Entities;

public class Member : BaseEntity
{
    public Guid MemberID { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string EmailID { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public MemberRoleEnum RoleName { get; set; }
    public Guid? PracticeID { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public string ModUser { get; set; } = string.Empty;
}
