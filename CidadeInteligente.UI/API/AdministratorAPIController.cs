﻿using CidadeInteligente.Application.Commands.CreateArea;
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
public class AdministratorAPIController(ILogger<AdministratorAPIController> logger, IMediator mediator) : ControllerBase {
    private readonly ILogger<AdministratorAPIController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    #region Users
    [HttpGet("users")]
    public async Task<ActionResult> GetAllUsers() {
        GetAllUsersQuery getAllUsersQuery = new();
        List<UserViewModel> users = await this._mediator.Send(getAllUsersQuery);
        return this.Ok(users);
    }

    [HttpGet("users/{userId}")]
    public async Task<ActionResult> GetUserById(long userId) {
        GetUserByIdQuery getUserByIdQuery = new(userId);
        UserViewModel user = await this._mediator.Send(getUserByIdQuery);
        return this.Ok(user);
    }

    [HttpPost("users")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand command) {
        long userId = await this._mediator.Send(command);
        return this.CreatedAtAction(nameof(this.GetUserById), new { userId }, command);
    }

    [HttpPatch("users/{userId}")]
    public async Task<ActionResult> UpdateUser(long userId, [FromBody] UpdateUserCommand command) {
        command.UserId = userId;
        await this._mediator.Send(command);
        return this.NoContent();
    }

    [HttpDelete("users/{userId}")]
    public async Task<ActionResult> DeleteUser(long userId) {
        DeleteUserByIdCommand deleteUserByIdCommand = new(userId);
        await this._mediator.Send(deleteUserByIdCommand);
        return this.NoContent();
    }
    #endregion

    #region Areas
    [HttpGet("areas")]
    public async Task<ActionResult> GetAllAreas() {
        GetAllAreasQuery getAllAreasQuery = new();
        List<AreaViewModel> areas = await this._mediator.Send(getAllAreasQuery);
        return this.Ok(areas);
    }

    [HttpGet("areas/{areaId}")]
    public async Task<ActionResult> GetAreaById(long areaId) {
        GetAreaByIdQuery getAreaByIdQuery = new(areaId);
        AreaViewModel area = await this._mediator.Send(getAreaByIdQuery);
        return this.Ok(area);
    }

    [HttpPost("areas")]
    public async Task<ActionResult> CreateArea([FromBody] CreateAreaCommand command) {
        long areaId = await this._mediator.Send(command);
        return this.CreatedAtAction(nameof(this.GetAreaById), new { areaId }, command);
    }

    [HttpPatch("areas/{areaId}")]
    public async Task<ActionResult> UpdateArea(long areaId, [FromBody] UpdateAreaCommand command) {
        command.AreaId = areaId;
        await this._mediator.Send(command);
        return this.NoContent();
    }

    [HttpDelete("areas/{areaId}")]
    public async Task<ActionResult> DeleteArea(long areaId) {
        DeleteAreaByIdCommand deleteAreaByIdCommand = new(areaId);
        await this._mediator.Send(deleteAreaByIdCommand);
        return this.NoContent();
    }
    #endregion

    #region Courses
    [HttpGet("courses")]
    public async Task<ActionResult> GetAllCourses() {
        GetAllCoursesQuery getAllCoursesQuery = new();
        List<CourseViewModel> courses = await this._mediator.Send(getAllCoursesQuery);
        return this.Ok(courses);
    }

    [HttpGet("courses/{courseId}")]
    public async Task<ActionResult> GetCourseById(long courseId) {
        GetCourseByIdQuery getCourseByIdQuery = new(courseId);
        CourseViewModel course = await this._mediator.Send(getCourseByIdQuery);
        return this.Ok(course);
    }

    [HttpPost("courses")]
    public async Task<ActionResult> CreateCourse([FromBody] CreateCourseCommand command) {
        long courseId = await this._mediator.Send(command);
        return this.CreatedAtAction(nameof(this.GetCourseById), new { courseId }, command);
    }

    [HttpPatch("courses/{courseId}")]
    public async Task<ActionResult> UpdateCourse(long courseId, [FromBody] UpdateCourseCommand command) {
        command.CourseId = courseId;
        await this._mediator.Send(command);
        return this.NoContent();
    }

    [HttpDelete("courses/{courseId}")]
    public async Task<ActionResult> DeleteCourse(long courseId) {
        DeleteCourseByIdCommand deleteCourseByIdCommand = new(courseId);
        await this._mediator.Send(deleteCourseByIdCommand);
        return this.NoContent();
    }
    #endregion

    #region Projects
    [HttpPost("projects")]
    public async Task<ActionResult> CreateProject([FromBody] CreateProjectCommand command) {
        command.CreatorUserId = this.User.UserId();
        long projectId = await this._mediator.Send(command);
        this.Response.Headers.Location = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host}/ver-projeto/{projectId}";
        return this.StatusCode(201);
    }

    [HttpPatch("projects/{projectId}")]
    public async Task<ActionResult> UpdateProject(long projectId, [FromBody] UpdateProjectCommand command) {
        command.ProjectId = projectId;
        command.UserIdEditor = this.User.UserId();
        await this._mediator.Send(command);
        return this.NoContent();
    }

    [HttpDelete("projects/{projectId}")]
    public async Task<ActionResult> DeleteProject(long projectId) {
        DeleteProjectByIdCommand deleteProjectByIdCommand = new(projectId) {
            UserIdEditor = this.User.UserId()
        };
        await this._mediator.Send(deleteProjectByIdCommand);
        return this.NoContent();
    }
    #endregion
}