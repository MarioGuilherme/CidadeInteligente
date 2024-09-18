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
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.UI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.API;

[Route("API/admin")]
[Authorize(Roles = nameof(Role.Teacher))]
public class AdministratorAPIController(ILogger<AdministratorAPIController> logger, IMediator mediator) : ControllerBase {
    private readonly ILogger<AdministratorAPIController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    #region Users
    [HttpGet("users")]
    public async Task<ActionResult> GetAllUsers() {
        try {
            GetAllUsersQuery getAllUsersQuery = new();
            List<UserViewModel> users = await this._mediator.Send(getAllUsersQuery);
            return this.Ok(users);
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }

    [HttpGet("users/{userId}")]
    public async Task<ActionResult> GetUserById(long userId) {
        try {
            GetUserByIdQuery getUserByIdQuery = new(userId);
            UserViewModel user = await this._mediator.Send(getUserByIdQuery);
            return this.Ok(user);
        } catch (UserNotExistException) {
            return this.NotFound();
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }

    [HttpPost("users")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand command) {
        try {
            long userId = await this._mediator.Send(command);
            return this.CreatedAtAction(nameof(this.GetUserById), new { userId }, command);
        } catch (EmailAlreadyInUseException) {
            return this.Conflict();
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }

    [HttpPatch("users/{userId}")]
    public async Task<ActionResult> UpdateUser(long userId, [FromBody] UpdateUserCommand command) {
        try {
            command.UserId = userId;
            await this._mediator.Send(command);
            return this.NoContent();
        } catch (EmailAlreadyInUseException) {
            return this.Conflict();
        } catch (UserNotExistException) {
            return this.NotFound();
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }

    [HttpDelete("users/{userId}")]
    public async Task<ActionResult> DeleteUser(long userId) {
        try {
            DeleteUserByIdCommand deleteUserByIdCommand = new(userId);
            await this._mediator.Send(deleteUserByIdCommand);
            return this.NoContent();
        } catch (UserNotExistException) {
            return this.NotFound();
        } catch (UserWithDepedentProjectsException) {
            return this.Conflict();
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }
    #endregion

    #region Areas
    [HttpGet("areas")]
    public async Task<ActionResult> GetAllAreas() {
        try {
            GetAllAreasQuery getAllAreasQuery = new();
            List<AreaViewModel> areas = await this._mediator.Send(getAllAreasQuery);
            return this.Ok(areas);
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }

    [HttpGet("areas/{areaId}")]
    public async Task<ActionResult> GetAreaById(long areaId) {
        try {
            GetAreaByIdQuery getAreaByIdQuery = new(areaId);
            AreaViewModel area = await this._mediator.Send(getAreaByIdQuery);
            return this.Ok(area);
        } catch (AreaNotExistException) {
            return this.NotFound();
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }

    [HttpPost("areas")]
    public async Task<ActionResult> CreateArea([FromBody] CreateAreaCommand command) {
        try {
            long areaId = await this._mediator.Send(command);
            return this.CreatedAtAction(nameof(this.GetAreaById), new { areaId }, command);
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }

    [HttpPatch("areas/{areaId}")]
    public async Task<ActionResult> UpdateArea(long areaId, [FromBody] UpdateAreaCommand command) {
        try {
            command.AreaId = areaId;
            await this._mediator.Send(command);
            return this.NoContent();
        } catch (AreaNotExistException) {
            return this.NotFound();
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }

    [HttpDelete("areas/{areaId}")]
    public async Task<ActionResult> DeleteArea(long areaId) {
        try {
            DeleteAreaByIdCommand deleteAreaByIdCommand = new(areaId);
            await this._mediator.Send(deleteAreaByIdCommand);
            return this.NoContent();
        } catch (AreaNotExistException) {
            return this.NotFound();
        } catch (AreaWithDepedentProjectsException) {
            return this.Conflict();
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }
    #endregion

    #region Courses
    [HttpGet("courses")]
    public async Task<ActionResult> GetAllCourses() {
        try {
            GetAllCoursesQuery getAllCoursesQuery = new();
            List<CourseViewModel> courses = await this._mediator.Send(getAllCoursesQuery);
            return this.Ok(courses);
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }

    [HttpGet("courses/{courseId}")]
    public async Task<ActionResult> GetCourseById(long courseId) {
        try {
            GetCourseByIdQuery getCourseByIdQuery = new(courseId);
            CourseViewModel course = await this._mediator.Send(getCourseByIdQuery);
            return this.Ok(course);
        } catch (AreaNotExistException) {
            return this.NotFound();
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }

    [HttpPost("courses")]
    public async Task<ActionResult> CreateCourse([FromBody] CreateCourseCommand command) {
        try {
            long courseId = await this._mediator.Send(command);
            return this.CreatedAtAction(nameof(this.GetCourseById), new { courseId }, command);
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }

    [HttpPatch("courses/{courseId}")]
    public async Task<ActionResult> UpdateCourse(long courseId, [FromBody] UpdateCourseCommand command) {
        try {
            command.CourseId = courseId;
            await this._mediator.Send(command);
            return this.NoContent();
        } catch (CourseNotExistException) {
            return this.NotFound();
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }

    [HttpDelete("courses/{courseId}")]
    public async Task<ActionResult> DeleteCourse(long courseId) {
        try {
            DeleteCourseByIdCommand deleteCourseByIdCommand = new(courseId);
            await this._mediator.Send(deleteCourseByIdCommand);
            return this.NoContent();
        } catch (CourseNotExistException) {
            return this.NotFound();
        } catch (CourseWithDepedentProjectsException) {
            return this.Conflict();
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }
    #endregion

    #region Projects
    [HttpPost("projects")]
    public async Task<ActionResult> CreateProject([FromBody] CreateProjectCommand command) {
        try {
            command.CreatorUserId = this.User.UserId();
            long projectId = await this._mediator.Send(command);
            this.Response.Headers.Location = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host}/ver-projeto/{projectId}";
            return this.StatusCode(201);
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }

    [HttpPatch("projects/{projectId}")]
    public async Task<ActionResult> UpdateProject(long projectId, [FromBody] UpdateProjectCommand command) {
        try {
            command.ProjectId = projectId;
            await this._mediator.Send(command);
            return this.NoContent();
        } catch (ProjectNotExistException) {
            return this.NotFound();
        } catch (UserIsReadOnlyException) {
            return this.Forbid();
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }

    [HttpDelete("projects/{projectId}")]
    public async Task<ActionResult> DeleteProject(long projectId) {
        try {
            DeleteProjectByIdCommand deleteProjectByIdCommand = new(projectId);
            await this._mediator.Send(deleteProjectByIdCommand);
            return this.NoContent();
        } catch (ProjectNotExistException) {
            return this.NotFound();
        } catch (UserIsReadOnlyException) {
            return this.Forbid();
        } catch (Exception ex) {
            this._logger.LogError("{Message}", ex.Message);
            return this.StatusCode(500);
        }
    }
    #endregion
}