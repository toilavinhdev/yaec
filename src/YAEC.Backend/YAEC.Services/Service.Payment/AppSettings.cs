using Package.Payments.VnPay;
using Package.Shared.Extensions;

namespace Service.Payment;

public class AppSettings : IAppSettings
{
    public VnPayOptions VnPayOptions { get; set; } = null!;
}