using StackExchange.Redis;

namespace Package.Redis;

public interface IRedisService
{
    Task<string?> StringGetAsync(string key);

    Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = null);
}

public class RedisService : IRedisService
{
    private readonly IConnectionMultiplexer _connection;
    
    private IDatabase Database() => _connection.GetDatabase();
    
    private IServer Server()
    {
        foreach (var endpoint in _connection.GetEndPoints())
        {
            var server = _connection.GetServer(endpoint);
            if (!server.IsReplica) return server;
        }
        throw new RedisException("Redis master database was not found");
    }
    
    public RedisService(IConnectionMultiplexer connection)
    {
        _connection = connection;
    }
    
    public async Task<string?> StringGetAsync(string key)
    {
        var database = Database();
        var value = await database.StringGetAsync(key);
        return value.ToString();
    }
    
    public async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = null)
    {
        var database = Database();
        return await database.StringSetAsync(key, value, expiry);
    }
}