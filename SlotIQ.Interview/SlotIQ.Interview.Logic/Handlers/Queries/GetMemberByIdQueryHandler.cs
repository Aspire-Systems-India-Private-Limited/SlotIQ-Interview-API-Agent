using AutoMapper;
using Microsoft.Extensions.Logging;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Data.Repositories.Contracts;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Queries;

namespace SlotIQ.Interview.Logic.Handlers.Queries;

/// <summary>
/// Handler for GetMemberByIdQuery
/// </summary>
public class GetMemberByIdQueryHandler : IQueryHandler<GetMemberByIdQuery, Result<MemberDto>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetMemberByIdQueryHandler> _logger;

    public GetMemberByIdQueryHandler(
        IMemberRepository memberRepository,
        IMapper mapper,
        ILogger<GetMemberByIdQueryHandler> logger)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<MemberDto>> Handle(GetMemberByIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Retrieving member with ID: {MemberID}", query.Id);

            var result = await _memberRepository.GetByIdAsync(query.Id);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Member with ID {MemberID} not found", query.Id);
                return Result<MemberDto>.Failure(result.Error ?? "Member not found");
            }

            var dto = _mapper.Map<MemberDto>(result.Value);

            _logger.LogInformation("Successfully retrieved member with ID: {MemberID}", query.Id);
            return Result<MemberDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling GetMemberByIdQuery for ID: {MemberID}", query.Id);
            return Result<MemberDto>.Failure("An error occurred while retrieving the member");
        }
    }
}
