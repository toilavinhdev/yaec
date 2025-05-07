using Microsoft.Extensions.DependencyInjection;

namespace Package.Shared.Extensions;

public static class CorsExtensions
{
    public const string AllowAll = "_allowAll";
    
    public static void AddCoreCors(this IServiceCollection services)
    {
        services.AddCors(o =>
        {
            o.AddPolicy(AllowAll, b =>
            {
                b.AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(_ => true);
            });
        });
    }
}