namespace SlotIQ.Interview.API.Models;

/// <summary>
/// Response model for updating a member
/// </summary>
public class UpdateMemberResponse
{
    public Guid MemberID { get; set; }
    public string SuccessCode { get; set; } = string.Empty;
    public string SuccessMessage { get; set; } = string.Empty;
}
