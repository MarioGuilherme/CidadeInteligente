using CidadeInteligente.Application.Queries.GetAllAreas;
using CidadeInteligente.Application.Queries.GetAllCourses;
using CidadeInteligente.Application.Queries.GetAllUsers;
using CidadeInteligente.Core.Enums;
using CidadeInteligente.UI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.UI.Controllers;

[Route("admin")]
[Authorize(Roles = nameof(Role.Teacher))]
public class AdministratorController(ILogger<AdministratorController> logger, IMediator mediator) : Controller
{
    private readonly ILogger<AdministratorController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    public async Task<ViewResult> Index()
    {
        try
        {
            GetAllUsersQuery getAllUsersQuery = new();
            GetAllAreasQuery getAllAreasQuery = new();
            GetAllCoursesQuery getAllCoursesQuery = new();

            ViewBag.Users = await _mediator.Send(getAllUsersQuery);
            ViewBag.Areas = await _mediator.Send(getAllAreasQuery);
            ViewBag.Courses = await _mediator.Send(getAllCoursesQuery);

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }
}