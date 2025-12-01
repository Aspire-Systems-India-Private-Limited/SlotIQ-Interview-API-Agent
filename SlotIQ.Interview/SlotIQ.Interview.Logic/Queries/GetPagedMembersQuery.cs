using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.Logic.Queries;

/// <summary>
/// Query for retrieving paged Members
/// </summary>
public record GetPagedMembersQuery(int PageNumber, int PageSize) : IQuery<PaginatedResult<MemberDto>>;
