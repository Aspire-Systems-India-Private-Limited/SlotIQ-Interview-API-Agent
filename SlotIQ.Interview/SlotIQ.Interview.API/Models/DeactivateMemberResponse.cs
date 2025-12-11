namespace SlotIQ.Interview.API.Models;

/// <summary>
/// Response model for member deactivation
/// </summary>
public class DeactivateMemberResponse
{
    /// <summary>
    /// ID of the deactivated member
    /// </summary>
    public Guid MemberID { get; set; }

    /// <summary>
    /// Success code
    /// </summary>
    public string SuccessCode { get; set; } = string.Empty;

    /// <summary>
    /// Success message
    /// </summary>
    public string SuccessMessage { get; set; } = string.Empty;
}
