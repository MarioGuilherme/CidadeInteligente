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

    [HttpGet]
    public async Task<ViewResult> Index([FromQuery] string tab)
    {
        try
        {
            GetUsersQueryResult getUsersQueryResult = await _mediator.Send(new GetUsersQuery());
            GetAreasQueryResult getAreasQueryResult = await _mediator.Send(new GetAreasQuery());
            GetCoursesQueryResult getCoursesQueryResult = await _mediator.Send(new GetCoursesQuery());

            ViewBag.Users = getUsersQueryResult.Users;
            ViewBag.Areas = getAreasQueryResult.Areas;
            ViewBag.Courses = getCoursesQueryResult.Courses;

            ViewBag.Tab = tab;

            return View();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "{Message}");
            return View("~/Views/Error.cshtml", new ErrorViewModel(500));
        }
    }
}