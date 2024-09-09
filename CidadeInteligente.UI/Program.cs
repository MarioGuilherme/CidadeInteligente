using CidadeInteligente.UI.Filters;
using CidadeInteligente.Application.Queries.GetAllProjects;
using CidadeInteligente.Application.Validators;
using CidadeInteligente.Core.Repositories;
using CidadeInteligente.Infrastructure.Persistence;
using CidadeInteligente.Infrastructure.Persistence.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using CidadeInteligente.Core.Services;
using CidadeInteligente.Infrastructure.Auth;
using CidadeInteligente.Infrastructure.CloudServices;
using AutoMapper;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.UI.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Environment.SetEnvironmentVariable("AzureStorageConnectionString", builder.Configuration["AzureStorage:ConnectionString"]!);
Environment.SetEnvironmentVariable("AzureStorageContainerName", builder.Configuration["AzureStorage:ContainerName"]!);
Environment.SetEnvironmentVariable("AzureStorageBaseURL", builder.Configuration["AzureStorage:BaseURL"]!);

builder.Services.AddDbContext<CidadeInteligenteDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CidadeInteligenteDb")));

builder.Services.AddScoped<IFileStorage, AzureStorageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton(new MapperConfiguration(config => {
    config.CreateMap<User, UserViewModel>()
          .ForMember(uvm => uvm.Course, o => o.MapFrom(u => u.Course.Description))
          .ForMember(uvm => uvm.Role, o => o.MapFrom(u => u.Role.GetDescription()));

    config.CreateMap<Project, ProjectDetailsViewModel>()
          .ForMember(pvm => pvm.Area, o => o.MapFrom(p => p.Area.Description))
          .ForMember(pvm => pvm.Course, o => o.MapFrom(p => p.Course.Description))
          .ForMember(pvm => pvm.Description, o => o.MapFrom(p => p.Description ?? "Sem descri��o"));

    config.CreateMap<User, ProjectUserViewModel>();
    config.CreateMap<Area, AreaViewModel>();
    config.CreateMap<Course, CourseViewModel>();
    config.CreateMap<Project, ProjectViewModel>();
    config.CreateMap<Media, MediaViewModel>();
    config.CreateMap<Media, MediaDetailsViewModel>();
}).CreateMapper());

builder.Services.AddControllers(options => options.Filters.Add(typeof(ValidationFilter)));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateAreaCommandValidator>();

builder.Services.AddMediatR(opt => opt.RegisterServicesFromAssemblyContaining(typeof(GetAllProjectsQuery)));

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services
    .AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options => options.LoginPath = "/login");

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

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