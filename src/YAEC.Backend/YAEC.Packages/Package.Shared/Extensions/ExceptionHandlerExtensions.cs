using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Package.Shared.Exceptions;
using Package.Shared.ValueObjects;

namespace Package.Shared.Extensions;

public static class ExceptionHandlerExtensions
{
    public static void UseCoreExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(errApp =>
        {
            errApp.Run(async httpContext =>
            {
                var feature = httpContext.Features.Get<IExceptionHandlerFeature>();

                if (feature is not null)
                {
                    var exception = feature.Error;
                    httpContext.Response.ContentType = "application/problem+json";
                    httpContext.Response.StatusCode = exception switch
                    {
                        BusinessExceptions => (int)HttpStatusCode.BadRequest,
                        _ => (int)HttpStatusCode.InternalServerError,
                    };
                    await httpContext.Response.WriteAsJsonAsync(
                        new ApiResponse()
                        {
                            Code = exception switch
                            {
                                BusinessExceptions => (int)HttpStatusCode.BadRequest,
                                _ => (int)HttpStatusCode.InternalServerError,
                            },
                            Message = exception.Message
                        });
                }
            });
        });
    }
}