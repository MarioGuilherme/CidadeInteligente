using CidadeInteligente.Application.Queries.GetAllAreas;
using CidadeInteligente.Application.Queries.GetAllCourses;
using CidadeInteligente.Application.Queries.GetAllUsers;
using CidadeInteligente.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.UI.Controllers;

[Route("admin")]
[Authorize(Roles = nameof(Role.Teacher))]
public class AdministratorController(ILogger<AdministratorController> logger, IMediator mediator) : Controller {
    private readonly ILogger<AdministratorController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    public async Task<ViewResult> Index() {
        GetAllUsersQuery getAllUsersQuery = new();
        GetAllAreasQuery getAllAreasQuery = new();
        GetAllCoursesQuery getAllCoursesQuery = new();

        this.ViewBag.Users = await this._mediator.Send(getAllUsersQuery);
        this.ViewBag.Areas = await this._mediator.Send(getAllAreasQuery);
        this.ViewBag.Courses = await this._mediator.Send(getAllCoursesQuery);

        return this.View();
    }
}