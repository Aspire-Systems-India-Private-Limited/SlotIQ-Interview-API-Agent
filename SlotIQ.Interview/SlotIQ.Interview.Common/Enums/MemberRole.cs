using System.Text.Json.Serialization;

namespace SlotIQ.Interview.Common.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MemberRole
{
    MasterAdmin = 1,
    PracticeAdmin = 2,
    TechPanelMember = 3,
    TAAdmin = 4,
    TAPanelMember = 5
}
