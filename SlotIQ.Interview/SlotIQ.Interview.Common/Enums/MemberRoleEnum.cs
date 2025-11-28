using System.Text.Json.Serialization;

namespace SlotIQ.Interview.Common.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MemberRoleEnum
{
    MasterAdmin = 1,
    PracticeAdmin = 2,
    TechTeamMember = 3,
    TATeamAdmin = 4
}
