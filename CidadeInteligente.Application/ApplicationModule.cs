using CidadeInteligente.Application.Commands.LoginUser;
using CidadeInteligente.Application.Notifications;
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
            services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<LoginUserCommand>());

            return services;
        }

        private IServiceCollection AddFluentValidation()
        {
            services
                .AddFluentValidationAutoValidation(o => o.DisableDataAnnotationsValidation = true)
                .AddValidatorsFromAssemblyContaining<LoginUserCommandValidator>();

            return services;
        }

        private IServiceCollection AddNotification()
        {
            services.AddScoped<INotificationContext, NotificationContext>();

            return services;
        }
    }
}
