using Package.Logger;
using Package.Observability;
using Package.OpenApi;
using Package.Shared.Extensions;
using Package.Shared.Mediator;
using Service.Identity;

var builder = WebApplication.CreateBuilder(args).WithEnvironment<AppSettings>();
var services = builder.Services;
services.AddCoreLogger();
services.AddHttpContextAccessor();
services.AddCoreCors();
services.AddOpenApi(typeof(Program).Assembly);
services.AddCoreMediator(typeof(Program).Assembly);
services.AddObservability();

var app = builder.Build();
app.UseObservability();
app.UseCoreExceptionHandler();
app.UseCors(CorsExtensions.AllowAll);
app.UseOpenApi();
app.MapGet("/", () => "Service.Identity");
app.Run();