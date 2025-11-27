using System.Text.Json.Serialization;

namespace SlotIQ.Interview.Common.Enums;

/// <summary>
/// Role identifier for member permissions and access control
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MemberRoleEnum
{
    MasterAdmin = 1,
    PracticeAdmin = 2,
    TechPanelMember = 3,
    TAAdmin = 4,
    TAPanelMember = 5
}
