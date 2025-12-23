using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SlotIQ.Interview.API.Models;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Logic.Commands;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Handlers.Commands;

namespace SlotIQ.Interview.API.Endpoints;

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
            .Produces<ApiResponse<object>>(409)
            .Produces<ApiResponse<object>>(500);
    }

    private static async Task<Results<Created<ApiResponse<CreateMemberResponse>>, BadRequest<ApiResponse<object>>, Conflict<ApiResponse<object>>, StatusCodeHttpResult>> CreateMember(
        CreateMemberRequest request,
        CreateMemberCommandHandler handler,
        IMapper mapper,
        CancellationToken ct)
    {
        var createMemberDto = mapper.Map<CreateMemberDto>(request);
        var result = await handler.Handle(new CreateMemberCommand(createMemberDto), ct);

        if (!result.IsSuccess)
        {
            // Check if it's a duplicate error (409 Conflict)
            if (result.Error?.Contains("already exists", StringComparison.OrdinalIgnoreCase) == true ||
                result.Error?.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true)
            {
                return TypedResults.Conflict(new ApiResponse<object>
                {
                    Success = false,
                    ErrorMessage = result.Error
                });
            }

            // Check if it's a validation error (400 Bad Request)
            return TypedResults.BadRequest(new ApiResponse<object>
            {
                Success = false,
                ErrorMessage = result.Error
            });
        }

        var response = new CreateMemberResponse
        {
            MemberID = result.Value!.MemberID,
            SuccessCode = "MEMBER_ONBOARD_SUCCESS",
            SuccessMessage = "User onboarded successfully."
        };

        return TypedResults.Created(
            $"/slotiq/v1/members/{result.Value!.MemberID}",
            new ApiResponse<CreateMemberResponse>
            {
                Success = true,
                Data = response,
                SuccessCode = "MEMBER_ONBOARD_SUCCESS",
                SuccessMessage = "User onboarded successfully."
            });
    }
}
