using Azure.Storage.Blobs;
using CidadeInteligente.Core.Repositories;
using CidadeInteligente.Core.Services;
using CidadeInteligente.Infrastructure.CloudServices;
using CidadeInteligente.Infrastructure.Persistence;
using CidadeInteligente.Infrastructure.Persistence.Repositories;
using CidadeInteligente.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CidadeInteligente.Infrastructure;

public static class InfrastructureModule {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        services
            .AddPersistence(configuration)
            .AddRepositories()
            .AddUnitOfWork()
            .AddCookieAuthentication(configuration)
            .AddServices(configuration);

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) {
        var connectionString = configuration.GetConnectionString("DevFreelaCs");

        services.AddDbContext<CidadeInteligenteDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("CidadeInteligenteDb")));
        //services.AddDbContext<CidadeInteligenteDbContext>(options => options.UseInMemoryDatabase("CidadeInteligenteDb")); // Banco em memória

        return services;
    }
    private static IServiceCollection AddRepositories(this IServiceCollection services) {
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAreaRepository, AreaRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();

        return services;
    }

    private static IServiceCollection AddCookieAuthentication(this IServiceCollection services, IConfiguration configuration) {
        services
            .AddAuthentication()
            .AddCookie("Cookie", options => {
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
                options.AccessDeniedPath = "/sem-permissao";
            });

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) {
        Environment.SetEnvironmentVariable("AzureStorageBlobURL", $"{configuration["AzureStorage:BaseURL"]!}/{configuration["AzureStorage:ContainerName"]!}");

        services.AddSingleton<IFileStorage, AzureStorageService>(_ => {
            string connectionString = configuration["AzureStorage:ConnectionString"]!;
            string containerName = configuration["AzureStorage:ContainerName"]!;
            BlobContainerClient blobContainerClient = new(connectionString, containerName);
            blobContainerClient.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            return new(connectionString, containerName);
        });
        services.AddSingleton<IEmailService, SendGridEmailService>(_ => {
            string apiKey = configuration["SendGrid:ApiKey"]!;
            string senderEmail = configuration["SendGrid:SenderEmail"]!;
            return new(apiKey, senderEmail);
        });

        services.AddHttpContextAccessor();

        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services) {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}