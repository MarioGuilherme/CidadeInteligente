using CidadeInteligente.Application.Queries.GetAreas;
using CidadeInteligente.Application.Queries.GetCourses;
using CidadeInteligente.Application.Queries.GetUsers;
using CidadeInteligente.Core.Enums;
using CidadeInteligente.Mvc.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CidadeInteligente.Mvc.Controllers;

[Route("admin")]
[Authorize(Roles = nameof(Role.Teacher))]
public class AdminController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    public async Task<ViewResult> Index()
    {
        try
        {
            GetUsersQuery getAllUsersQuery = new();
            GetAreasQuery getAreasQuery = new();
            GetCoursesQuery getAllCoursesQuery = new();

            ViewBag.Users = await _mediator.Send(getAllUsersQuery);
            ViewBag.Areas = await _mediator.Send(getAreasQuery);
            ViewBag.Courses = await _mediator.Send(getAllCoursesQuery);

            return View();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "{Message}");
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }
}