using SlotIQ.Interview.Common.Enums;

namespace SlotIQ.Interview.Data.Entities;

/// <summary>
/// Base entity pattern with audit fields for all entities
/// </summary>
public abstract class BaseEntity
{
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public string ModifiedBy { get; set; } = string.Empty;
    public SourceEnum Source { get; set; }
}
