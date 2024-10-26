using CidadeInteligente.Application;
using CidadeInteligente.Infrastructure;
using CidadeInteligente.Infrastructure.Persistence;
using CidadeInteligente.UI.ExceptionHandler;
using CidadeInteligente.UI.Filters;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers(options => options.Filters.Add(typeof(ValidationFilter)));
#if DEBUG
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
#else
builder.Services.AddControllersWithViews();
#endif

WebApplication app = builder.Build();

#region Cria o banco de dados na inicialização
using AsyncServiceScope asyncServiceScope = app.Services.CreateAsyncScope();
IServiceProvider services = asyncServiceScope.ServiceProvider;
CidadeInteligenteDbContext context = services.GetRequiredService<CidadeInteligenteDbContext>();
await context.Database.MigrateAsync();
#endregion

if (!app.Environment.IsDevelopment()) {
    app.UseHsts();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Projects}/{action=Index}/{id?}"
);

app.Run();
