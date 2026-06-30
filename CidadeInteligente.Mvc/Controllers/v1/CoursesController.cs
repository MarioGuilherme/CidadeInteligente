using CidadeInteligente.Application.Commands.CreateCourse;
using CidadeInteligente.Application.Commands.DeleteCourseById;
using CidadeInteligente.Application.Commands.UpdateCourse;
using CidadeInteligente.Application.Queries.GetCourseById;
using CidadeInteligente.Application.Queries.GetCourses;
using CidadeInteligente.Core.Enums;
using CidadeInteligente.Mvc.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.Mvc.Controllers.v1;

[Route("api/v1/courses")]
[Authorize(Roles = nameof(Role.Teacher))]
public class CoursesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult> GetAllCourses()
    {
        GetCoursesQuery getAllCoursesQuery = new();
        GetCoursesQueryResult getAllCoursesQueryResult = await _mediator.Send(getAllCoursesQuery);
        return Ok(getAllCoursesQueryResult.Courses);
    }

    [HttpGet("{courseId}")]
    public async Task<ActionResult> GetCourseById(long courseId)
    {
        GetCourseByIdQuery getCourseByIdQuery = new(courseId);
        GetCourseByIdQueryResult getCourseByIdQueryResult = await _mediator.Send(getCourseByIdQuery);
        return Ok(getCourseByIdQueryResult);
    }

    [HttpPost]
    public async Task<ActionResult> CreateCourse([FromBody] CreateCourseCommand command)
    {
        long courseId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCourseById), new { courseId }, command);
    }

    [HttpPatch("{courseId}")]
    public async Task<ActionResult> UpdateCourse(long courseId, [FromBody] UpdateCourseRequest request)
    {
        UpdateCourseCommand updateCourseCommand = new(courseId, request.Description);
        await _mediator.Send(updateCourseCommand);
        return NoContent();
    }

    [HttpDelete("{courseId}")]
    public async Task<ActionResult> DeleteCourse(long courseId)
    {
        DeleteCourseByIdCommand deleteCourseByIdCommand = new(courseId);
        await _mediator.Send(deleteCourseByIdCommand);
        return NoContent();
    }
}