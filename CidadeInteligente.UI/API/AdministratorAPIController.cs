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
using CidadeInteligente.Application.Queries.GetAreaById;
using CidadeInteligente.Application.Queries.GetAreas;
using CidadeInteligente.Application.Queries.GetCourseById;
using CidadeInteligente.Application.Queries.GetCourses;
using CidadeInteligente.Application.Queries.GetUserById;
using CidadeInteligente.Application.Queries.GetUsers;
using CidadeInteligente.Core.Enums;
using CidadeInteligente.UI.Extensions;
using CidadeInteligente.UI.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.UI.API;

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
        GetUsersQuery getAllUsersQuery = new();
        GetUsersQueryResult getUsersQueryResult = await _mediator.Send(getAllUsersQuery);
        return Ok(getUsersQueryResult.Users);
    }

    [HttpGet("users/{userId}")]
    public async Task<ActionResult> GetUserById(long userId)
    {
        GetUserByIdQuery getUserByIdQuery = new(userId);
        GetUserByIdQueryResult getUserByIdQueryResult = await _mediator.Send(getUserByIdQuery);
        return Ok(getUserByIdQueryResult);
    }

    [HttpPost("users")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        long userId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUserById), new { userId }, command);
    }

    [HttpPatch("users/{userId}")]
    public async Task<ActionResult> UpdateUser(long userId, [FromBody] UpdateUserRequest request)
    {
        UpdateUserCommand updateUserCommand = new(userId, request.CourseId, request.Name, request.Email, request.Role);
        await _mediator.Send(updateUserCommand);
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
        GetAreasQuery getAllAreasQuery = new();
        GetAreasQueryResult getAreasQueryResult = await _mediator.Send(getAllAreasQuery);
        return Ok(getAreasQueryResult.Areas);
    }

    [HttpGet("areas/{areaId}")]
    public async Task<ActionResult> GetAreaById(long areaId)
    {
        GetAreaByIdQuery getAreaByIdQuery = new(areaId);
        GetAreaByIdQueryResult getAreaByIdQueryResult = await _mediator.Send(getAreaByIdQuery);
        return Ok(getAreaByIdQueryResult);
    }

    [HttpPost("areas")]
    public async Task<ActionResult> CreateArea([FromBody] CreateAreaCommand command)
    {
        long areaId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAreaById), new { areaId }, command);
    }

    [HttpPatch("areas/{areaId}")]
    public async Task<ActionResult> UpdateArea(long areaId, [FromBody] UpdateAreaRequest request)
    {
        UpdateAreaCommand updateAreaCommand = new(areaId, request.Description);
        await _mediator.Send(updateAreaCommand);
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
        GetCoursesQuery getAllCoursesQuery = new();
        GetCoursesQueryResult getAllCoursesQueryResult = await _mediator.Send(getAllCoursesQuery);
        return Ok(getAllCoursesQueryResult.Courses);
    }

    [HttpGet("courses/{courseId}")]
    public async Task<ActionResult> GetCourseById(long courseId)
    {
        GetCourseByIdQuery getCourseByIdQuery = new(courseId);
        GetCourseByIdQueryResult getCourseByIdQueryResult = await _mediator.Send(getCourseByIdQuery);
        return Ok(getCourseByIdQueryResult);
    }

    [HttpPost("courses")]
    public async Task<ActionResult> CreateCourse([FromBody] CreateCourseCommand command)
    {
        long courseId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCourseById), new { courseId }, command);
    }

    [HttpPatch("courses/{courseId}")]
    public async Task<ActionResult> UpdateCourse(long courseId, [FromBody] UpdateCourseRequest request)
    {
        UpdateCourseCommand updateCourseCommand = new(courseId, request.Description);
        await _mediator.Send(updateCourseCommand);
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
    public async Task<ActionResult> CreateProject([FromBody] CreateProjectRequest request)
    {
        CreateProjectCommand createProjectCommand = new(request.Title,
            request.AreaId,
            request.CourseId,
            User.UserId(),
            request.Description,
            request.StartedAt,
            request.FinishedAt,
            request.InvolvedUsers,
            request.Medias.Select(m => new CreateProjectCommand.CreateMediaCommand(m.Title,
                m.Description,
                m.Extension,
                m.Base64)));
        long projectId = await _mediator.Send(createProjectCommand);
        Response.Headers.Location = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/ver-projeto/{projectId}";
        return StatusCode(201);
    }

    [HttpPatch("projects/{projectId:int}")]
    public async Task<ActionResult> UpdateProject(long projectId, [FromBody] UpdateProjectRequest request)
    {
        UpdateProjectCommand updateProjectCommand = new(projectId,
            User.UserId(),
            request.Title,
            request.AreaId,
            request.CourseId,
            request.Description,
            request.StartedAt,
            request.FinishedAt,
            request.InvolvedUsers,
            request.Medias.Select(media => new UpdateProjectCommand.UpdateMediaCommand(media.MediaId,
                media.Title,
                media.Description,
                media.Extension,
                media.Path,
                media.Base64)));
        await _mediator.Send(updateProjectCommand);
        return NoContent();
    }

    [HttpDelete("projects/{projectId:int}")]
    public async Task<ActionResult> DeleteProject(long projectId)
    {
        DeleteProjectByIdCommand deleteProjectByIdCommand = new(projectId, User.UserId());
        await _mediator.Send(deleteProjectByIdCommand);
        return NoContent();
    }
    #endregion
}