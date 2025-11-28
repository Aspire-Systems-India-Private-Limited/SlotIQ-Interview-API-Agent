using SlotIQ.Interview.Common.Enums;

namespace SlotIQ.Interview.Data.Entities;

public abstract class BaseEntity
{
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string ModifiedBy { get; set; } = string.Empty;
    public SourceEnum Source { get; set; }
}
