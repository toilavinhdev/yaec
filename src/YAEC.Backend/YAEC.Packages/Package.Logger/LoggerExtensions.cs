using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Package.Logger;

public static class LoggerExtensions
{
    public static void AddCoreLogger(this IServiceCollection services)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        services.AddSerilog(logger);
    }
}