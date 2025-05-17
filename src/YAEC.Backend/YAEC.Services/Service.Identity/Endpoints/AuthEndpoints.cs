using Microsoft.AspNetCore.Mvc;
using Package.OpenApi.MinimalApi;
using Package.Shared.Mediator;
using Service.Identity.Application.UserModule.Commands;

namespace Service.Identity.Endpoints;

public class AuthEndpoints : IEndpoints
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/auth")
            .WithTags("Auth");
        V1(group);
        V2(group);
    }

    private static void V1(RouteGroupBuilder group)
    {
        group.MapPost("/sign-up", async
            ([FromServices] IMediator mediator, [FromBody] SignUpCommand command) => await mediator.SendAsync(command))
            .WithSummary("Register an account")
            .MapToApiVersion(1);

        group.MapPost("/sign-in", async
            ([FromServices] IMediator mediator, [FromBody] SignInCommand command) => await mediator.SendAsync(command))
            .WithSummary("Login your account")
            .MapToApiVersion(1);
    }
    
    private static void V2(RouteGroupBuilder group)
    {
        group.MapPost("/sign-in", async
            ([FromServices] IMediator mediator) =>
            {
                await Task.CompletedTask;
                return Results.Ok("OK");
            })
            .WithSummary("Login your account")
            .MapToApiVersion(2);
    }
}