using AutoMapper;
using Azure.Storage.Blobs;
using CidadeInteligente.Application.Queries.GetAllProjects;
using CidadeInteligente.Application.Validators;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using CidadeInteligente.Core.Services;
using CidadeInteligente.Infrastructure.Auth;
using CidadeInteligente.Infrastructure.CloudServices;
using CidadeInteligente.Infrastructure.Persistence;
using CidadeInteligente.Infrastructure.Persistence.Repositories;
using CidadeInteligente.UI.Extensions;
using CidadeInteligente.UI.Filters;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Environment.SetEnvironmentVariable("AzureStorageBlobURL", $"{builder.Configuration["AzureStorage:BaseURL"]!}/{builder.Configuration["AzureStorage:ContainerName"]!}");

builder.Services.AddDbContext<CidadeInteligenteDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CidadeInteligenteDb")));

builder.Services.AddSingleton<IFileStorage, AzureStorageService>(f => {
    string connectionString = builder.Configuration["AzureStorage:ConnectionString"]!;
    string containerName = builder.Configuration["AzureStorage:ContainerName"]!;
    BlobContainerClient blobContainerClient = new(connectionString, containerName);
    blobContainerClient.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
    return new(connectionString, containerName);
});
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton(new MapperConfiguration(config => {
    config.CreateMap<User, UserViewModel>()
          .ForMember(uvm => uvm.Course, o => o.MapFrom(u => u.Course.Description))
          .ForMember(uvm => uvm.RoleDescription, o => o.MapFrom(u => u.Role.GetDescription()));
    config.CreateMap<User, LoginViewModel>()
          .ForMember(uvm => uvm.Role, o => o.MapFrom(u => u.Role.GetDescription()));

    config.CreateMap<Project, ProjectDetailsViewModel>()
          .ForMember(pvm => pvm.Area, o => o.MapFrom(p => p.Area.Description))
          .ForMember(pvm => pvm.Course, o => o.MapFrom(p => p.Course.Description));

    config.CreateMap<User, ProjectUserViewModel>();
    config.CreateMap<Area, AreaViewModel>();
    config.CreateMap<Course, CourseViewModel>();
    config.CreateMap<Project, ProjectViewModel>();
    config.CreateMap<Media, MediaViewModel>();
    config.CreateMap<Media, MediaDetailsViewModel>();
}).CreateMapper());

builder.Services.AddControllers(options => options.Filters.Add(typeof(ValidationFilter)));
builder.Services.AddFluentValidationAutoValidation(opt => opt.DisableDataAnnotationsValidation = true);
builder.Services.AddValidatorsFromAssemblyContaining<CreateAreaCommandValidator>();

builder.Services.AddMediatR(opt => opt.RegisterServicesFromAssemblyContaining(typeof(GetAllProjectsQuery)));

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services
    .AddAuthentication()
    .AddCookie("Cookie", options => {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/sem-permissao";
    });

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