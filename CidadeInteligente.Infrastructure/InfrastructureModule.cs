using Azure.Storage.Blobs;
using CidadeInteligente.Core.Repositories;
using CidadeInteligente.Core.Services;
using CidadeInteligente.Infrastructure.CloudServices;
using CidadeInteligente.Infrastructure.Persistence;
using CidadeInteligente.Infrastructure.Persistence.Repositories;
using CidadeInteligente.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CidadeInteligente.Infrastructure;

public static class InfrastructureModule
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services
                .AddPersistence(configuration)
                .AddRepositories()
                .AddUnitOfWork()
                .AddAuthentication()
                .AddFileStorage(configuration)
                .AddEmailService(configuration);

            return services;
        }

        private IServiceCollection AddPersistence(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("CidadeInteligenteDb")!;
            services.AddDbContext<CidadeInteligenteDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }

        private IServiceCollection AddRepositories()
        {
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAreaRepository, AreaRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();

            return services;
        }

        private IServiceCollection AddUnitOfWork()
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        private IServiceCollection AddAuthentication()
        {
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.LogoutPath = "/logout";
                    options.AccessDeniedPath = "/forbidden";
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });

            return services;
        }

        private IServiceCollection AddFileStorage(IConfiguration configuration)
        {
            Environment.SetEnvironmentVariable("AzureStorageBlobURL", $"{configuration["AzureStorage:BaseURL"]!}/{configuration["AzureStorage:ContainerName"]!}");

            services.AddSingleton<IFileStorage, AzureStorageService>(_ =>
            {
                string connectionString = configuration["AzureStorage:ConnectionString"]!;
                string containerName = configuration["AzureStorage:ContainerName"]!;
                BlobContainerClient blobContainerClient = new(connectionString, containerName);
                blobContainerClient.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
                return new(connectionString, containerName);
            });

            return services;
        }

        private IServiceCollection AddEmailService(IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddSingleton<IEmailService, SendGridEmailService>(_ =>
            {
                string apiKey = configuration["SendGrid:ApiKey"]!;
                string senderEmail = configuration["SendGrid:SenderEmail"]!;
                return new(apiKey, senderEmail);
            });

            return services;
        }
    }
}
