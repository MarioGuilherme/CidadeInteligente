using CidadeInteligente.Application.Commands.CreateProject;
using CidadeInteligente.Application.Commands.DeleteProjectById;
using CidadeInteligente.Application.Commands.UpdateProject;
using CidadeInteligente.Core.Enums;
using CidadeInteligente.Mvc.Extensions;
using CidadeInteligente.Mvc.Requests.v1;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.Mvc.Controllers.v1;

[Route("api/v1/projects")]
[Authorize(Roles = nameof(Role.Teacher))]
public class ProjectsApiController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult> CreateProject([FromForm] CreateProjectRequest request)
    {
        CreateProjectCommand createProjectCommand = new(request.Title,
            request.AreaId,
            request.CourseId,
            User.UserId!.Value,
            request.Description,
            request.StartedAt,
            request.FinishedAt,
            request.InvolvedUsers,
            request.Medias.Select(m => new CreateProjectCommand.CreateMediaCommand(m.Title,
                m.Description,
                Path.GetExtension(m.File.FileName),
                m.File.OpenReadStream)));
        int projectId = await _mediator.Send(createProjectCommand);
        Response.Headers.Location = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/projects/{projectId}/view";
        return StatusCode(201);
    }

    [HttpPatch("{projectId:int}")]
    public async Task<ActionResult> UpdateProject(int projectId, [FromForm] UpdateProjectRequest request)
    {
        UpdateProjectCommand updateProjectCommand = new(projectId,
            User.UserId,
            request.Title,
            request.AreaId,
            request.CourseId,
            request.Description,
            request.StartedAt,
            request.FinishedAt,
            request.InvolvedUsers,
            request.Medias.Select(m => new UpdateProjectCommand.UpdateMediaCommand(m.MediaId,
                m.Title,
                m.Description,
                Path.GetExtension(m.File.FileName),
                m.File.OpenReadStream)));
        await _mediator.Send(updateProjectCommand);
        return NoContent();
    }

    [HttpDelete("{projectId:int}")]
    public async Task<ActionResult> DeleteProject(int projectId)
    {
        DeleteProjectByIdCommand deleteProjectByIdCommand = new(projectId, User.UserId);
        await _mediator.Send(deleteProjectByIdCommand);
        return NoContent();
    }
}