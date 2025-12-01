using System.Text.Json.Serialization;

namespace SlotIQ.Interview.Common.Enums;

/// <summary>
/// Source system making the change for audit tracking
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SourceEnum
{
    Web = 1,
    Mobile = 2,
    API = 3,
    Thirdparty = 4,
    Manual = 5
}
