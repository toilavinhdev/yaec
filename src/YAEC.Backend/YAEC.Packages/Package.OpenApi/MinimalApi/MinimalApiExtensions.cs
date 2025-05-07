using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Package.OpenApi.Swagger;

namespace Package.OpenApi.MinimalApi;

internal static class MinimalApiExtensions
{
    public static void AddCoreMinimalApis(this IServiceCollection services, Assembly assembly)
    {
        services
            .AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.ReportApiVersions = true;
                x.AssumeDefaultVersionWhenUnspecified = false;
                x.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Api-Version"));
            })
            .AddApiExplorer(x =>
            {
                x.GroupNameFormat = "'v'V";
                x.SubstituteApiVersionInUrl = true;
            });
        var serviceDescriptors = assembly.DefinedTypes
            .Where(type =>
                !type.IsAbstract
                && !type.IsInterface
                && type.IsAssignableTo(typeof(IEndpoints)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoints), type));
        services.TryAddEnumerable(serviceDescriptors);
    }
    
    public static void UseCoreMinimalApis(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        var versionedGroup = app
            .MapGroup("/api/v{apiVersion:apiVersion}")
            .WithApiVersionSet(apiVersionSet);
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoints>>();
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(versionedGroup);
        }
    }
}