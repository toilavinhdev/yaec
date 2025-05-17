using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Package.Redis;

public static class RedisExtensions
{
    public static void AddRedis(this IServiceCollection services)
    {
        services.AddSingleton<IRedisService, RedisService>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<RedisOptions>();
            var lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options.Url));
            var connection = lazyConnection.Value;
            return new RedisService(connection);
        });
    }
}