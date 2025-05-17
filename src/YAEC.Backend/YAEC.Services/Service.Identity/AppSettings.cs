using Package.MongoDb;
using Package.Shared.Extensions;

namespace Service.Identity;

public class AppSettings : IAppSettings
{
    public MongoDbOptions MongoDbOptions { get; set; } = null!;
}