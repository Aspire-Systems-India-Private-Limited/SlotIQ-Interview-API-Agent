using AutoMapper;
using Microsoft.Extensions.Logging;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Queries;

namespace SlotIQ.Interview.Logic.Handlers.Queries;

/// <summary>
/// Handler for GetMembersPagedQuery
/// </summary>
public class GetMembersPagedQueryHandler : IQueryHandler<GetMembersPagedQuery, PaginatedResult<MemberDto>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetMembersPagedQueryHandler> _logger;

    public GetMembersPagedQueryHandler(
        IMemberRepository memberRepository,
        IMapper mapper,
        ILogger<GetMembersPagedQueryHandler> logger)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedResult<MemberDto>> Handle(GetMembersPagedQuery query, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Retrieving paged members: Page {PageNumber}, Size {PageSize}, Sort {SortBy} {SortOrder}",
                query.PageNumber, query.PageSize, query.SortBy, query.SortOrder);

            var result = await _memberRepository.GetMembersPagedAsync(
                query.PageNumber,
                query.PageSize,
                query.SortBy,
                query.SortOrder,
                query.IsActive,
                query.RoleName,
                query.PracticeID);

            var dtos = _mapper.Map<IEnumerable<MemberDto>>(result.Items);

            return new PaginatedResult<MemberDto>
            {
                Items = dtos,
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling GetMembersPagedQuery");
            return new PaginatedResult<MemberDto>();
        }
    }
}
