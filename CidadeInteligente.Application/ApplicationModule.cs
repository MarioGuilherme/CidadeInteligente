using AutoMapper;
using CidadeInteligente.Application.Extensions;
using CidadeInteligente.Application.Queries.GetAllProjects;
using CidadeInteligente.Application.Validators;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace CidadeInteligente.Application;

public static class ApplicationModule {
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.AddMediatR(opt => opt.RegisterServicesFromAssemblyContaining(typeof(GetAllProjectsQuery)));
        services.AddFluentValidationAutoValidation(opt => opt.DisableDataAnnotationsValidation = true);
        services.AddValidatorsFromAssemblyContaining<CreateAreaCommandValidator>();

        #region AutoMapper
        services.AddSingleton(new MapperConfiguration(config => {
            config.CreateMap<User, UserViewModel>()
                  .ForMember(uvm => uvm.Course, o => o.MapFrom(u => u.Course.Description))
                  .ForMember(uvm => uvm.RoleDescription, o => o.MapFrom(u => u.Role.GetDescription()));

            config.CreateMap<User, LoginViewModel>()
                  .ForMember(lvm => lvm.Role, o => o.MapFrom(u => u.Role.GetDescription()));

            config.CreateMap<User, UserDataChangePassword>()
                  .ConstructUsing(u => new(u.Name, u.TokenRecoverPassword!));

            config.CreateMap<Project, ProjectDetailsViewModel>()
                  .ForMember(pdvm => pdvm.Area, o => o.MapFrom(p => p.Area.Description))
                  .ForMember(pdvm => pdvm.Course, o => o.MapFrom(p => p.Course.Description));

            config.CreateMap<User, ProjectUserViewModel>();
            config.CreateMap<Area, AreaViewModel>();
            config.CreateMap<Course, CourseViewModel>();
            config.CreateMap<Project, ProjectViewModel>();
            config.CreateMap<Media, MediaViewModel>();
            config.CreateMap<Media, MediaDetailsViewModel>();
        }).CreateMapper());
        #endregion

        return services;
    }
}