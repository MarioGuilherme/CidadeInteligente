using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CidadeInteligente.Application.Options;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Services;
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
            string connectionString = configuration.GetConnectionString("Database")!;
            services.AddDbContext<CidadeInteligenteDbContext>(options => options.UseSqlServer(connectionString));

            services.AddOptions<PaginationOptions>().BindConfiguration("Pagination");

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

            services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();

            return services;
        }

        private IServiceCollection AddFileStorage(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("FileStorage");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                services.AddSingleton<IFileStorage, NoOpFileStorage>();
                return services;
            }

            string containerName = configuration["FileStorage:ContainerName"] ?? "project-medias";
            BlobContainerClient blobContainerClient = new(connectionString, containerName);
            blobContainerClient.CreateIfNotExists(PublicAccessType.Blob);

            services.Configure<FileStorageOptions>(options =>
            {
                options.ConnectionString = connectionString;
                options.BaseUrl = blobContainerClient.Uri.ToString();
            });
            services.AddSingleton<IFileStorage, AzureStorageService>(_ => new(blobContainerClient));
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
