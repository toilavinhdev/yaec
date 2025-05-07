using Package.OpenApi;
using Package.Shared.Extensions;
using Package.Shared.Mediator;
using Service.Identity;

var builder = WebApplication.CreateBuilder(args).WithEnvironment<AppSettings>();
var services = builder.Services;
services.AddHttpContextAccessor();
services.AddCoreCors();
services.AddCoreOpenApi(typeof(Program).Assembly);
services.AddCoreMediator(typeof(Program).Assembly);

var app = builder.Build();
app.UseCors(CorsExtensions.AllowAll);
app.UseHttpsRedirection();
app.UseCoreOpenApi();
app.MapGet("/", () => "Service.Identity");

app.Run();