using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Logic.Dtos;

namespace SlotIQ.Interview.Logic.Queries;

/// <summary>
/// Query for retrieving a Member by ID
/// </summary>
public record GetMemberByIdQuery(Guid Id) : IQuery<Result<MemberDto>>;
