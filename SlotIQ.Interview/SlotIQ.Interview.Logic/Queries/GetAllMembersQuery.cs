using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.Logic.Queries;

/// <summary>
/// Query for retrieving all Members
/// </summary>
public record GetAllMembersQuery() : IQuery<Result<IEnumerable<MemberDto>>>;
