namespace SlotIQ.Interview.API.Models;

public class CreateMemberResponse
{
    public Guid MemberID { get; set; }
    public string SuccessCode { get; set; } = string.Empty;
    public string SuccessMessage { get; set; } = string.Empty;
}
