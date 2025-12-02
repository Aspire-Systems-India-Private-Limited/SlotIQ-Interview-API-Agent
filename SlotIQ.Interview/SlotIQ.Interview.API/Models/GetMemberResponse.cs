using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.API.Models;

/// <summary>
/// Response model for getting a single member by ID
/// </summary>
public class GetMemberResponse
{
    public string SuccessCode { get; set; } = string.Empty;
    public string SuccessMessage { get; set; } = string.Empty;
    public MemberDto? Member { get; set; }
}
