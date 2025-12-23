using Microsoft.AspNetCore.Mvc;
using SlotIQ.Interview.Logic.Commands;
using SlotIQ.Interview.Logic.Dtos;
using SlotIQ.Interview.Logic.Handlers.Commands;

namespace SlotIQ.Interview.API.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        var authGroup = app.MapGroup("/api/auth")
            .WithTags("Authentication");

        authGroup.MapPost("/login", LoginAsync)
            .WithName("Login")
            .Produces<MemberLoginResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .Produces(StatusCodes.Status400BadRequest)
            .AllowAnonymous();
    }

    private static async Task<IResult> LoginAsync(
        [FromBody] MemberLoginRequest request,
        [FromServices] MemberLoginCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new MemberLoginCommand
        {
            UserNameOrEmail = request.UserNameOrEmail,
            Password = request.Password
        };

        var result = await handler.HandleAsync(command, cancellationToken);

        if (!result.IsSuccess)
        {
            var errorMessage = result.Error ?? "Login failed.";
            if (errorMessage.Contains("Password", StringComparison.OrdinalIgnoreCase))
            {
                return Results.UnprocessableEntity(new { error = errorMessage });
            }
            else if (errorMessage.Contains("Username", StringComparison.OrdinalIgnoreCase) || errorMessage.Contains("email", StringComparison.OrdinalIgnoreCase))
            {
                return Results.BadRequest(new { error = errorMessage });
            }
            else if (errorMessage.Contains("not found", StringComparison.OrdinalIgnoreCase))
            {
                return Results.NotFound(new { error = errorMessage });
            }
            else if (errorMessage.Contains("Invalid", StringComparison.OrdinalIgnoreCase) || errorMessage.Contains("not active", StringComparison.OrdinalIgnoreCase))
            {
                return Results.Unauthorized();
            }
            else
            {
                return Results.BadRequest(new { error = errorMessage });
            }
        }

        // Map MemberLoginResponseDto to MemberLoginResponse for API response
        var dto = result.Value!;
        var response = new MemberLoginResponse
        {
            Token = dto.Token,
            Member = dto.Member
        };
        return Results.Ok(response);
    }
}
