using Package.S3Manager;
using Package.Shared.Extensions;

namespace Service.Storage;

public class AppSettings : IAppSettings
{
    public S3Options S3Options { get; set; } = null!;
}