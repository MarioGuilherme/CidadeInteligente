using CidadeInteligente.Application.Commands.CreateArea;
using CidadeInteligente.Application.Commands.DeleteAreaById;
using CidadeInteligente.Application.Commands.UpdateArea;
using CidadeInteligente.Application.Queries.GetAreaById;
using CidadeInteligente.Application.Queries.GetAreas;
using CidadeInteligente.Core.Enums;
using CidadeInteligente.Mvc.Requests.v1;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.Mvc.Controllers.v1;

[Route("api/v1/areas")]
[Authorize(Roles = nameof(Role.Teacher))]
public class AreasController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult> GetAllAreas()
    {
        GetAreasQuery getAllAreasQuery = new();
        GetAreasQueryResult getAreasQueryResult = await _mediator.Send(getAllAreasQuery);
        return Ok(getAreasQueryResult.Areas);
    }

    [HttpGet("{areaId:int}")]
    public async Task<ActionResult> GetAreaById(int areaId)
    {
        GetAreaByIdQuery getAreaByIdQuery = new(areaId);
        GetAreaByIdQueryResult? getAreaByIdQueryResult = await _mediator.Send(getAreaByIdQuery);
        return Ok(getAreaByIdQueryResult);
    }

    [HttpPost]
    public async Task<ActionResult> CreateArea([FromBody] CreateAreaCommand command)
    {
        int? areaId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAreaById), new { areaId }, command);
    }

    [HttpPatch("{areaId:int}")]
    public async Task<ActionResult> UpdateArea(int areaId, [FromBody] UpdateAreaRequest request)
    {
        UpdateAreaCommand updateAreaCommand = new(areaId, request.Description);
        await _mediator.Send(updateAreaCommand);
        return NoContent();
    }

    [HttpDelete("{areaId:int}")]
    public async Task<ActionResult> DeleteArea(int areaId)
    {
        DeleteAreaByIdCommand deleteAreaByIdCommand = new(areaId);
        await _mediator.Send(deleteAreaByIdCommand);
        return NoContent();
    }
}