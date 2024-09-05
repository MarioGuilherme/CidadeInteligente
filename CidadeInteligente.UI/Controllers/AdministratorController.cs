using CidadeInteligente.Application.Queries.GetAllAreas;
using CidadeInteligente.Application.Queries.GetAllCourse;
using CidadeInteligente.Application.Queries.GetAllUsers;
using CidadeInteligente.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.UI.Controllers;

[Route("admin")]
//[Authorize(Roles = "Teacher")]
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

    [HttpGet("areas")]
    public async Task<ViewResult> Areas() {
        GetAllAreasQuery getAllAreasQuery = new();
        List<Area> areas = await this._mediator.Send(getAllAreasQuery);

        return this.View(areas);
    }

    [HttpGet("courses")]
    public async Task<ViewResult> Courses() {
        GetAllCoursesQuery getAllCoursesQuery = new();
        List<Course> courses = await this._mediator.Send(getAllCoursesQuery);

        return this.View(courses);
    }
}