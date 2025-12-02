using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SlotIQ.Interview.API.Models;
using SlotIQ.Interview.Common.Constants;
using SlotIQ.Interview.Common.Enums;
using SlotIQ.Interview.Common.Models;
using SlotIQ.Interview.Logic.Commands;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Handlers.Commands;
using SlotIQ.Interview.Logic.Handlers.Queries;
using SlotIQ.Interview.Logic.Queries;

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

        group.MapGet("/", GetMembers)
            .WithName("GetMembers")
            .WithSummary("Get all members with pagination")
            .WithDescription("Retrieve paginated list of members (FR#MAP-3)")
            .Produces<ApiResponse<GetMembersResponse>>(200)
            .Produces<ApiResponse<object>>(400)
            .Produces<ApiResponse<object>>(401)
            .Produces<ApiResponse<object>>(403)
            .Produces<ApiResponse<object>>(500);

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

    private static async Task<Results<Ok<ApiResponse<GetMembersResponse>>, BadRequest<ApiResponse<object>>>> GetMembers(
        GetMembersPagedQueryHandler handler,
        HttpContext httpContext,
        int pageNumber = 1,
        int pageSize = 25,
        string sortBy = "CreatedDate",
        string sortOrder = "DESC",
        bool? isActive = null,
        MemberRoleEnum? roleName = null,
        Guid? practiceID = null,
        CancellationToken ct = default)
    {
        // Validate pagination parameters
        if (pageNumber < 1)
        {
            return TypedResults.BadRequest(new ApiResponse<object>
            {
                Success = false,
                ErrorMessage = "PageNumber must be greater than 0",
                ErrorCode = ErrorCodes.ValidationError
            });
        }

        if (pageSize < 1 || pageSize > 200)
        {
            return TypedResults.BadRequest(new ApiResponse<object>
            {
                Success = false,
                ErrorMessage = "PageSize must be between 1 and 200",
                ErrorCode = ErrorCodes.ValidationError
            });
        }

        // Validate sortBy field (whitelist approach)
        var validSortFields = new[] { "UserName", "Firstname", "Lastname", "EmailAddress", "RoleName", "PracticeName", "CreatedDate" };
        if (!validSortFields.Contains(sortBy))
        {
            sortBy = "CreatedDate";
        }

        // Validate sortOrder
        if (sortOrder != "ASC" && sortOrder != "DESC")
        {
            sortOrder = "DESC";
        }

        var query = new GetMembersPagedQuery(
            pageNumber,
            pageSize,
            sortBy,
            sortOrder,
            isActive,
            roleName,
            practiceID);

        var result = await handler.Handle(query, ct);

        var response = new GetMembersResponse
        {
            SuccessCode = ErrorCodes.MemberListSuccess,
            SuccessMessage = ErrorMessages.MemberListSuccess,
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            HasNext = result.HasNextPage,
            HasPrevious = result.HasPreviousPage,
            Items = result.Items
        };

        return TypedResults.Ok(new ApiResponse<GetMembersResponse>
        {
            Success = true,
            Data = response
        });
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
            // Check if it's a not found error using the error constant
            if (result.Error == ErrorMessages.MemberNotFound)
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
