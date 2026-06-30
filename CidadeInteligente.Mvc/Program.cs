using CidadeInteligente.Application;
using CidadeInteligente.Infrastructure;
using CidadeInteligente.Infrastructure.Persistence;
using CidadeInteligente.Mvc.Filters;
using CidadeInteligente.Mvc.Middleware;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] (CorrelationId={CorrelationId}) {Message:lj} {NewLine}{Exception}")
    .CreateLogger();

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services
    .AddControllers(options => options.Filters.Add<NotificationResultFilter>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddHealthChecks();

#if DEBUG
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
#else
builder.Services.AddControllersWithViews();
#endif

WebApplication app = builder.Build();

using AsyncServiceScope asyncServiceScope = app.Services.CreateAsyncScope();
IServiceProvider services = asyncServiceScope.ServiceProvider;

#region Ensures the database is created at startup.
try
{
    CidadeInteligenteDbContext context = services.GetRequiredService<CidadeInteligenteDbContext>();
    if ((await context.Database.GetPendingMigrationsAsync()).Any())
        await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Error during database initialization.");
}
#endregion

app.UseSerilogRequestLogging();
app.UseMetricServer();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.MapMetrics();

app.MapHealthChecks("/health");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Projects}/{action=Index}/{id?}")
    .WithStaticAssets();

await app.RunAsync();

Log.CloseAndFlush();
