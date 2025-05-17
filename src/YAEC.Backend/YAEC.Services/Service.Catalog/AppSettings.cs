using Package.Redis;
using Package.Shared.Extensions;

namespace Service.Catalog;

public class AppSettings : IAppSettings
{
    public RedisOptions RedisOptions { get; set; } = null!;
}