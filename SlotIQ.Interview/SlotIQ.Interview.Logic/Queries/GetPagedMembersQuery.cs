using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.Logic.Queries;

/// <summary>
/// Query for retrieving paged Members with filters and sorting
/// </summary>
public record GetMembersPagedQuery(
    int PageNumber,
    int PageSize,
    string SortBy,
    string SortOrder,
    bool? IsActive,
    MemberRoleEnum? RoleName,
    Guid? PracticeID) : IQuery<PaginatedResult<MemberDto>>;
