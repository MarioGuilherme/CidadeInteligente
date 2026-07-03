using CidadeInteligente.Application.Notifications;
using CidadeInteligente.Application.Queries.AuthenticateUser;
using CidadeInteligente.Core.Notifications;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace CidadeInteligente.Application;

public static class ApplicationModule
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            services
                .AddMediatR()
                .AddFluentValidation()
                .AddNotification();

            return services;
        }

        private IServiceCollection AddMediatR()
        {
            services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<AuthenticateUserQuery>());

            return services;
        }

        private IServiceCollection AddFluentValidation()
        {
            services
                .AddFluentValidationAutoValidation(o => o.DisableDataAnnotationsValidation = true)
                .AddValidatorsFromAssemblyContaining<AuthenticateUserQueryValidator>();

            return services;
        }

        private IServiceCollection AddNotification()
        {
            services.AddScoped<INotificationContext, NotificationContext>();

            return services;
        }
    }
}
