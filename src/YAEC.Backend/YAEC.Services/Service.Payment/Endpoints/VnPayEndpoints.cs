using Microsoft.AspNetCore.Mvc;
using Package.OpenApi.MinimalApi;
using Package.Payments.VnPay;
using Package.Payments.VnPay.Models;

namespace Service.Payment.Endpoints;

public class VnPayEndpoints : IEndpoints
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/payment/vnpay")
            .WithTags("VnPay");
        V1(group);
    }
    
    private static void V1(RouteGroupBuilder group)
    {
        group.MapPost("/create-url",
            ([FromServices] IVnPayService vnPayService, [FromBody] CreateVnPayPaymentUrlRequest request) =>
            {
                var url = vnPayService.CreatePaymentUrl(request);
                return Results.Ok(url);
            })
            .WithSummary("Tạo URL thanh toán VNPay")
            .MapToApiVersion(1);

        group.MapGet("/return",
            ([FromServices] IVnPayService vnPayService, HttpContext httpContext) =>
            {
                var parameters = httpContext.Request.Query
                    .ToDictionary(x => x.Key, x => x.Value.ToString());
                return Results.Ok(new
                {
                    IsValidSignature = vnPayService.ValidateSignature(parameters),
                    Parameters = parameters
                });
            })
            .WithSummary("VnPay redirect về ReturnUrl để hiển thị thông tin")
            .MapToApiVersion(1);
        
        group.MapGet("/ipn",
                ([FromServices] IVnPayService vnPayService, HttpContext httpContext) =>
                {
                    var parameters = httpContext.Request.Query
                        .ToDictionary(x => x.Key, x => x.Value.ToString());
                    return Results.Ok(new
                    {
                        IsValidSignature = vnPayService.ValidateSignature(parameters),
                        Parameters = parameters
                    });
                })
            .WithSummary("VNPay call API IPN đã cài đặt trên Merchant để xử lý lưu database")
            .MapToApiVersion(1);
    }
}