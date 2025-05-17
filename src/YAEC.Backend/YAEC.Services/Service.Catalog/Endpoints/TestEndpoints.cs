using Microsoft.AspNetCore.Mvc;
using Package.OpenApi.MinimalApi;
using Package.Redis;

namespace Service.Catalog.Endpoints;

public class TestEndpoints : IEndpoints
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/test")
            .WithTags("Test");

        group.MapGet("/redis/get", async
            ([FromQuery] string key, [FromServices] IRedisService redisService) =>
            {
                return await redisService.StringGetAsync(key);
            })
            .MapToApiVersion(1);
        
        group.MapPost("/redis/set", async
            ([FromQuery] string key, [FromBody] string value, [FromServices] IRedisService redisService) =>
            {
                await redisService.StringSetAsync(key, value);
            })
            .MapToApiVersion(1);;
    }
}