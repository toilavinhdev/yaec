using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Package.OpenApi.MinimalApi;
using Package.OpenApi.Swagger;

namespace Package.OpenApi;

public static class OpenApiExtensions
{
    public static void AddOpenApi(this IServiceCollection services, Assembly assembly)
    {
        services.AddEndpointsApiExplorer();
        services.AddCoreMinimalApis(assembly);
        services.AddCoreSwagger(assembly.GetName().Name!);
    }

    public static void UseOpenApi(this WebApplication app, ApiVersionSet? apiVersionSet = null)
    {
        apiVersionSet ??= app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();
        app.UseCoreMinimalApis(apiVersionSet);
        app.UseCoreSwagger();
    }
}