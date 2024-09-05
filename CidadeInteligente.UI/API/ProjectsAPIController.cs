using CidadeInteligente.Application.Commands.CreateProject;
using CidadeInteligente.Application.Commands.DeleteProjectById;
using CidadeInteligente.Application.Commands.UpdateArea;
using CidadeInteligente.Application.Commands.UpdateProject;
using CidadeInteligente.Core.Enums;
using CidadeInteligente.UI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.UI.API;

[Route("API")]
[Authorize(Roles = nameof(Role.Teacher))]
public class ProjectsAPIController(ILogger<ProjectsAPIController> logger, IMediator mediator) : ControllerBase {
    private readonly ILogger<ProjectsAPIController> logger = logger;
    private readonly IMediator _mediator = mediator;

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

        Unit? unit = await this._mediator.Send(command);

        if (unit is null) return this.NotFound();

        return this.NoContent();
    }

    [HttpDelete("projects/{projectId}")]
    public async Task<ActionResult> DeleteProject(long projectId) {
        DeleteProjectByIdCommand deleteProjectByIdCommand = new(projectId);
        Unit? unit = await this._mediator.Send(deleteProjectByIdCommand);

        if (unit is null)
            return this.NotFound();

        return this.NoContent();
    }
}