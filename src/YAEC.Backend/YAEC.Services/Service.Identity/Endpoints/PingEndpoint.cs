using Package.OpenApi.MinimalApi;

namespace Service.Identity.Endpoints;

internal class PingEndpoint : IEndpoints
{
    private int _counterGlobal;
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/ping")
            .WithTags("Ping");
        V1(group);
    }

    private void V1(RouteGroupBuilder group)
    {
        var counterInner = 0;
        group.MapPost("/counter",
            () =>
            {
                _counterGlobal++;
                counterInner++;
                return Results.Ok($"{_counterGlobal}.{counterInner}");
            })
            .WithSummary("Health check")
            .MapToApiVersion(1);
    }
}