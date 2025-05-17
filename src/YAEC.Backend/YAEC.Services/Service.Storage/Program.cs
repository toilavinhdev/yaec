using Package.FFmpeg;
using Package.Logger;
using Package.OpenApi;
using Package.S3Manager;
using Package.Shared.Extensions;
using Service.Storage;

var builder = WebApplication.CreateBuilder(args).WithEnvironment<AppSettings>();
var services = builder.Services;
services.AddCoreLogger();
services.AddHttpContextAccessor();
services.AddCoreCors();
services.AddOpenApi(typeof(Program).Assembly);
services.AddS3Manager();

var app = builder.Build();
app.UseCoreExceptionHandler();
app.UseCors(CorsExtensions.AllowAll);
app.UseOpenApi();
app.MapGet("/", () => "Service.Storage");
await app.InitializeFFmpeg();
app.Run();