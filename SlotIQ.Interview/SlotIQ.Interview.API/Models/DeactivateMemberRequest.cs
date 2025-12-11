using SlotIQ.Interview.Common.Enums;

namespace SlotIQ.Interview.API.Models;

/// <summary>
/// Request model for deactivating a member
/// </summary>
public class DeactivateMemberRequest
{
    /// <summary>
    /// Optional reason for deactivation (max 250 characters)
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// User ID of the person deactivating the member
    /// </summary>
    public string UpdatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Application source identifier
    /// </summary>
    public SourceEnum Source { get; set; }
}
