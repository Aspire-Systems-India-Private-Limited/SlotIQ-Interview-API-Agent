using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SlotIQ.Interview.API.Models;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Logic.Commands;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Handlers.Commands;

namespace SlotIQ.Interview.API.Endpoints;

/// <summary>
/// Member management endpoints
/// </summary>
public static class MemberEndpoints
{
    public static void MapMemberEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/slotiq/v1/members")
            .WithTags("Members")
            .WithOpenApi();

        group.MapPost("/", CreateMember)
            .WithName("CreateMember")
            .WithSummary("Onboard new member")
            .WithDescription("Create a new member in the system (FR#MAP-1)")
            .Accepts<CreateMemberRequest>("application/json")
            .Produces<ApiResponse<CreateMemberResponse>>(201)
            .Produces<ApiResponse<object>>(400)
            .Produces<ApiResponse<object>>(401)
            .Produces<ApiResponse<object>>(403)
            .Produces<ApiResponse<object>>(409)
            .Produces<ApiResponse<object>>(500);
    }

    private static async Task<Results<Created<ApiResponse<CreateMemberResponse>>, BadRequest<ApiResponse<object>>>> CreateMember(
        CreateMemberRequest request,
        CreateMemberCommandHandler handler,
        IMapper mapper,
        HttpContext httpContext,
        CancellationToken ct)
    {
        // TODO: Get CreatedBy from authenticated user context
        var currentUser = "system"; // Placeholder - should come from authentication

        var createMemberDto = mapper.Map<CreateMemberDto>(request);
        createMemberDto.CreatedBy = currentUser;

        var result = await handler.Handle(new CreateMemberCommand(createMemberDto), ct);

        if (!result.IsSuccess)
        {
            return TypedResults.BadRequest(new ApiResponse<object>
            {
                Success = false,
                ErrorMessage = result.Error,
                ErrorCode = ErrorCodes.ValidationError
            });
        }

        var response = new CreateMemberResponse
        {
            MemberID = result.Value!.MemberID,
            SuccessCode = ErrorCodes.MemberOnboardSuccess,
            SuccessMessage = ErrorMessages.MemberOnboardSuccess
        };

        return TypedResults.Created($"/slotiq/v1/members/{result.Value!.MemberID}", new ApiResponse<CreateMemberResponse>
        {
            Success = true,
            Data = response
        });
    }
}
