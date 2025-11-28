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

        var (success, token, member, errorMessage) = await handler.HandleAsync(command, cancellationToken);

        if (!success)
        {
            // Determine the appropriate status code based on the error message
            if (errorMessage == "Password field is required and cannot be empty.")
            {
                return Results.UnprocessableEntity(new { error = errorMessage });
            }
            else if (errorMessage == "User not found.")
            {
                return Results.NotFound(new { error = errorMessage });
            }
            else if (errorMessage == "Invalid username/email or password.")
            {
                return Results.Unauthorized();
            }
            else if (errorMessage == "User account is not active.")
            {
                return Results.Unauthorized();
            }
            else
            {
                return Results.BadRequest(new { error = errorMessage });
            }
        }

        var response = new MemberLoginResponse
        {
            Token = token!,
            Member = member!
        };

        return Results.Ok(response);
    }
}
