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
            .WithOpenApi()
            .RequireAuthorization("MemberManagementPolicy");

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

        group.MapPut("/{memberid:guid}", UpdateMember)
            .WithName("UpdateMember")
            .WithSummary("Update member details")
            .WithDescription("Modify existing member details (FR#MAP-2)")
            .Accepts<UpdateMemberRequest>("application/json")
            .Produces<ApiResponse<UpdateMemberResponse>>(200)
            .Produces<ApiResponse<object>>(400)
            .Produces<ApiResponse<object>>(401)
            .Produces<ApiResponse<object>>(403)
            .Produces<ApiResponse<object>>(404)
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
    createMemberDto.Password = request.Password;
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

    private static async Task<Results<Ok<ApiResponse<UpdateMemberResponse>>, BadRequest<ApiResponse<object>>, NotFound<ApiResponse<object>>>> UpdateMember(
        Guid memberid,
        UpdateMemberRequest request,
        UpdateMemberCommandHandler handler,
        IMapper mapper,
        HttpContext httpContext,
        CancellationToken ct)
    {
        // TODO: Get UpdatedBy from authenticated user context
        var currentUser = "system"; // Placeholder - should come from authentication

        var updateMemberDto = mapper.Map<UpdateMemberDto>(request);
        updateMemberDto.ModifiedBy = currentUser;

        var result = await handler.Handle(new UpdateMemberCommand(memberid, updateMemberDto), ct);

        if (!result.IsSuccess)
        {
            // Check if it's a not found error
            if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
            {
                return TypedResults.NotFound(new ApiResponse<object>
                {
                    Success = false,
                    ErrorMessage = result.Error,
                    ErrorCode = ErrorCodes.ResourceNotFoundError
                });
            }

            return TypedResults.BadRequest(new ApiResponse<object>
            {
                Success = false,
                ErrorMessage = result.Error,
                ErrorCode = ErrorCodes.ValidationError
            });
        }

        var response = new UpdateMemberResponse
        {
            MemberID = result.Value!.MemberID,
            SuccessCode = ErrorCodes.MemberUpdateSuccess,
            SuccessMessage = ErrorMessages.MemberUpdateSuccess
        };

        return TypedResults.Ok(new ApiResponse<UpdateMemberResponse>
        {
            Success = true,
            Data = response
        });
    }
}
