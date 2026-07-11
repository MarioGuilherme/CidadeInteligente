using CidadeInteligente.Application.Commands.CreateCourse;
using CidadeInteligente.Application.Commands.DeleteCourseById;
using CidadeInteligente.Application.Commands.UpdateCourse;
using CidadeInteligente.Application.Queries.GetCourseById;
using CidadeInteligente.Application.Queries.GetCourses;
using CidadeInteligente.Domain.Enums;
using CidadeInteligente.Mvc.Requests.v1;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.Mvc.Controllers.v1;

[Route("api/v1/courses")]
[Authorize(Roles = nameof(Role.Teacher))]
[ApiController]
public class CoursesApiController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult> GetAllCourses()
    {
        GetCoursesQuery getAllCoursesQuery = new();
        GetCoursesQueryResult getAllCoursesQueryResult = await _mediator.Send(getAllCoursesQuery);
        return Ok(getAllCoursesQueryResult.Courses);
    }

    [HttpGet("{courseId:int}")]
    public async Task<ActionResult> GetCourseById(int courseId)
    {
        GetCourseByIdQuery getCourseByIdQuery = new(courseId);
        GetCourseByIdQueryResult? getCourseByIdQueryResult = await _mediator.Send(getCourseByIdQuery);
        return Ok(getCourseByIdQueryResult);
    }

    [HttpPost]
    public async Task<ActionResult> CreateCourse([FromBody] CreateCourseRequest request)
    {
        CreateCourseCommand createCourseCommand = new(request.Description);
        int? courseId = await _mediator.Send(createCourseCommand);
        return CreatedAtAction(nameof(GetCourseById), new { courseId }, default);
    }

    [HttpPatch("{courseId:int}")]
    public async Task<ActionResult> UpdateCourse(int courseId, [FromBody] UpdateCourseRequest request)
    {
        UpdateCourseCommand updateCourseCommand = new(courseId, request.Description);
        await _mediator.Send(updateCourseCommand);
        return NoContent();
    }

    [HttpDelete("{courseId:int}")]
    public async Task<ActionResult> DeleteCourse(int courseId)
    {
        DeleteCourseByIdCommand deleteCourseByIdCommand = new(courseId);
        await _mediator.Send(deleteCourseByIdCommand);
        return NoContent();
    }
}
