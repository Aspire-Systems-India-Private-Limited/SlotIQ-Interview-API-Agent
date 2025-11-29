namespace SlotIQ.Interview.Data.Entities;

public class Member : BaseEntity
{
    public Guid MemberID { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string EmailID { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public int RoleID { get; set; }
    public Guid PracticeID { get; set; }
}
