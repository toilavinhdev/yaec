using Microsoft.AspNetCore.Routing;

namespace Package.OpenApi.MinimalApi;

public interface IEndpoints
{
    void MapEndpoint(IEndpointRouteBuilder app);
}