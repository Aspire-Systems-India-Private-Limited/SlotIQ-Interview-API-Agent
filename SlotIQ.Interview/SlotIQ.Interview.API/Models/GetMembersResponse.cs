using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.API.Models;

/// <summary>
/// Response model for getting members with pagination
/// </summary>
public class GetMembersResponse
{
    public string SuccessCode { get; set; } = string.Empty;
    public string SuccessMessage { get; set; } = string.Empty;
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }
    public IEnumerable<MemberDto> Items { get; set; } = new List<MemberDto>();
}
