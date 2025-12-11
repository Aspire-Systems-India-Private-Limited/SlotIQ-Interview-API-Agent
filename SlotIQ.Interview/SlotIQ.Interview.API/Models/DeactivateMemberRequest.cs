using System.ComponentModel.DataAnnotations;
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
    [MaxLength(250, ErrorMessage = "Reason cannot exceed 250 characters")]
    public string? Reason { get; set; }

    /// <summary>
    /// User ID of the person deactivating the member
    /// </summary>
    [Required(ErrorMessage = "UpdatedBy is required")]
    public string UpdatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Application source identifier
    /// </summary>
    [Required(ErrorMessage = "Source is required")]
    public SourceEnum Source { get; set; }
}
