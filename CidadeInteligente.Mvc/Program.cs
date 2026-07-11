using CidadeInteligente.Application;
using CidadeInteligente.Infrastructure;
using CidadeInteligente.Infrastructure.Persistence;
using CidadeInteligente.Mvc.Filters;
using CidadeInteligente.Mvc.Middleware;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Serilog;
using System.Threading.RateLimiting;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services
    .AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("Database")!, name: "database");

builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    o.AddPolicy("auth", ctx => RateLimitPartition.GetFixedWindowLimiter(
        ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 5, Window = TimeSpan.FromMinutes(1) }));
});

IMvcBuilder mvcBuilder = builder.Services
    .AddControllersWithViews(options => options.Filters.Add<NotificationResultFilter>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

#if DEBUG
mvcBuilder.AddRazorRuntimeCompilation();
#endif

WebApplication app = builder.Build();

#region Ensures the database is created at startup.
using (AsyncServiceScope migrationScope = app.Services.CreateAsyncScope())
{
    CidadeInteligenteDbContext context = migrationScope.ServiceProvider.GetRequiredService<CidadeInteligenteDbContext>();
    try
    {
        if ((await context.Database.GetPendingMigrationsAsync()).Any())
            await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        ILogger<Program> logger = migrationScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogCritical(ex, "Error during database initialization.");
        throw;
    }
}
#endregion

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseSerilogRequestLogging();

app.UseRateLimiter();
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
