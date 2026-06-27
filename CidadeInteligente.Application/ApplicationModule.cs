using CidadeInteligente.Application.Queries.GetAllProjects;
using CidadeInteligente.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace CidadeInteligente.Application;

public static class ApplicationModule {
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.AddMediatR(opt => opt.RegisterServicesFromAssemblyContaining(typeof(GetAllProjectsQuery)));
        services.AddFluentValidationAutoValidation(opt => opt.DisableDataAnnotationsValidation = true);
        services.AddValidatorsFromAssemblyContaining<CreateAreaCommandValidator>();

        return services;
    }
}