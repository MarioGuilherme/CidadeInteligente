using CidadeInteligente.Application.Behaviors;
using CidadeInteligente.Application.Notifications;
using CidadeInteligente.Application.Options;
using CidadeInteligente.Application.Queries.AuthenticateUser;
using CidadeInteligente.Domain.Notifications;
using FluentValidation;
using MediatR;
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
                .AddNotification()
                .AddPaginationOptions();

            return services;
        }

        private IServiceCollection AddMediatR()
        {
            services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<AuthenticateUserQuery>());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        private IServiceCollection AddFluentValidation()
        {
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

            services.AddValidatorsFromAssemblyContaining<AuthenticateUserQueryValidator>();

            return services;
        }

        private IServiceCollection AddNotification()
        {
            services.AddScoped<INotificationContext, NotificationContext>();

            return services;
        }

        private IServiceCollection AddPaginationOptions()
        {
            services.AddOptions<PaginationOptions>().BindConfiguration("Pagination");

            return services;
        }
    }
}
