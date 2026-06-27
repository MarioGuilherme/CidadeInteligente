using CidadeInteligente.Application.Commands.CreateArea;
using CidadeInteligente.Application.Commands.CreateCourse;
using CidadeInteligente.Application.Commands.CreateProject;
using CidadeInteligente.Application.Commands.CreateUser;
using CidadeInteligente.Application.Commands.DeleteAreaById;
using CidadeInteligente.Application.Commands.DeleteCourseById;
using CidadeInteligente.Application.Commands.DeleteProjectById;
using CidadeInteligente.Application.Commands.DeleteUserById;
using CidadeInteligente.Application.Commands.UpdateArea;
using CidadeInteligente.Application.Commands.UpdateCourse;
using CidadeInteligente.Application.Commands.UpdateProject;
using CidadeInteligente.Application.Commands.UpdateUser;
using CidadeInteligente.Application.Queries.GetAllAreas;
using CidadeInteligente.Application.Queries.GetAllCourses;
using CidadeInteligente.Application.Queries.GetAllUsers;
using CidadeInteligente.Application.Queries.GetAreaById;
using CidadeInteligente.Application.Queries.GetCourseById;
using CidadeInteligente.Application.Queries.GetUserById;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Enums;
using CidadeInteligente.UI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.UI.Controllers;

[Route("API/admin")]
[Authorize(Roles = nameof(Role.Teacher))]
public class AdministratorAPIController(ILogger<AdministratorAPIController> logger, IMediator mediator) : ControllerBase
{
    private readonly ILogger<AdministratorAPIController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    #region Users
    [HttpGet("users")]
    public async Task<ActionResult> GetAllUsers()
    {
        GetAllUsersQuery getAllUsersQuery = new();
        List<UserViewModel> users = await _mediator.Send(getAllUsersQuery);
        return Ok(users);
    }

    [HttpGet("users/{userId}")]
    public async Task<ActionResult> GetUserById(long userId)
    {
        GetUserByIdQuery getUserByIdQuery = new(userId);
        UserViewModel user = await _mediator.Send(getUserByIdQuery);
        return Ok(user);
    }

    [HttpPost("users")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        long userId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUserById), new { userId }, command);
    }

    [HttpPatch("users/{userId}")]
    public async Task<ActionResult> UpdateUser(long userId, [FromBody] UpdateUserCommand command)
    {
        command.UserId = userId;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("users/{userId}")]
    public async Task<ActionResult> DeleteUser(long userId)
    {
        DeleteUserByIdCommand deleteUserByIdCommand = new(userId);
        await _mediator.Send(deleteUserByIdCommand);
        return NoContent();
    }
    #endregion

    #region Areas
    [HttpGet("areas")]
    public async Task<ActionResult> GetAllAreas()
    {
        GetAllAreasQuery getAllAreasQuery = new();
        List<AreaViewModel> areas = await _mediator.Send(getAllAreasQuery);
        return Ok(areas);
    }

    [HttpGet("areas/{areaId}")]
    public async Task<ActionResult> GetAreaById(long areaId)
    {
        GetAreaByIdQuery getAreaByIdQuery = new(areaId);
        AreaViewModel area = await _mediator.Send(getAreaByIdQuery);
        return Ok(area);
    }

    [HttpPost("areas")]
    public async Task<ActionResult> CreateArea([FromBody] CreateAreaCommand command)
    {
        long areaId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAreaById), new { areaId }, command);
    }

    [HttpPatch("areas/{areaId}")]
    public async Task<ActionResult> UpdateArea(long areaId, [FromBody] UpdateAreaCommand command)
    {
        command.AreaId = areaId;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("areas/{areaId}")]
    public async Task<ActionResult> DeleteArea(long areaId)
    {
        DeleteAreaByIdCommand deleteAreaByIdCommand = new(areaId);
        await _mediator.Send(deleteAreaByIdCommand);
        return NoContent();
    }
    #endregion

    #region Courses
    [HttpGet("courses")]
    public async Task<ActionResult> GetAllCourses()
    {
        GetAllCoursesQuery getAllCoursesQuery = new();
        List<CourseViewModel> courses = await _mediator.Send(getAllCoursesQuery);
        return Ok(courses);
    }

    [HttpGet("courses/{courseId}")]
    public async Task<ActionResult> GetCourseById(long courseId)
    {
        GetCourseByIdQuery getCourseByIdQuery = new(courseId);
        CourseViewModel course = await _mediator.Send(getCourseByIdQuery);
        return Ok(course);
    }

    [HttpPost("courses")]
    public async Task<ActionResult> CreateCourse([FromBody] CreateCourseCommand command)
    {
        long courseId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCourseById), new { courseId }, command);
    }

    [HttpPatch("courses/{courseId}")]
    public async Task<ActionResult> UpdateCourse(long courseId, [FromBody] UpdateCourseCommand command)
    {
        command.CourseId = courseId;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("courses/{courseId}")]
    public async Task<ActionResult> DeleteCourse(long courseId)
    {
        DeleteCourseByIdCommand deleteCourseByIdCommand = new(courseId);
        await _mediator.Send(deleteCourseByIdCommand);
        return NoContent();
    }
    #endregion

    #region Projects
    [HttpPost("projects")]
    public async Task<ActionResult> CreateProject([FromBody] CreateProjectCommand command)
    {
        command.CreatorUserId = User.UserId();
        long projectId = await _mediator.Send(command);
        Response.Headers.Location = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/ver-projeto/{projectId}";
        return StatusCode(201);
    }

    [HttpPatch("projects/{projectId}")]
    public async Task<ActionResult> UpdateProject(long projectId, [FromBody] UpdateProjectCommand command)
    {
        command.ProjectId = projectId;
        command.UserIdEditor = User.UserId();
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("projects/{projectId}")]
    public async Task<ActionResult> DeleteProject(long projectId)
    {
        DeleteProjectByIdCommand deleteProjectByIdCommand = new(projectId)
        {
            UserIdEditor = User.UserId()
        };
        await _mediator.Send(deleteProjectByIdCommand);
        return NoContent();
    }
    #endregion
}