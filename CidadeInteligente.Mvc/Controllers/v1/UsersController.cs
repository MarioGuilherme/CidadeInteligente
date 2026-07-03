using CidadeInteligente.Application.Commands.CreateUser;
using CidadeInteligente.Application.Commands.DeleteUserById;
using CidadeInteligente.Application.Commands.UpdateUser;
using CidadeInteligente.Application.Queries.GetUserById;
using CidadeInteligente.Application.Queries.GetUsers;
using CidadeInteligente.Domain.Enums;
using CidadeInteligente.Mvc.Requests.v1;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.Mvc.Controllers.v1;

[Route("api/v1/users")]
[Authorize(Roles = nameof(Role.Teacher))]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult> GetAllUsers()
    {
        GetUsersQuery getAllUsersQuery = new();
        GetUsersQueryResult getUsersQueryResult = await _mediator.Send(getAllUsersQuery);
        return Ok(getUsersQueryResult.Users);
    }

    [HttpGet("{userId:int}")]
    public async Task<ActionResult> GetUserById(int userId)
    {
        GetUserByIdQuery getUserByIdQuery = new(userId);
        GetUserByIdQueryResult? getUserByIdQueryResult = await _mediator.Send(getUserByIdQuery);
        return Ok(getUserByIdQueryResult);
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        CreateUserCommand createUserCommand = new(request.CourseId, request.Name, request.Email, request.Password, request.Role);
        int? userId = await _mediator.Send(createUserCommand);
        return CreatedAtAction(nameof(GetUserById), new { userId }, createUserCommand);
    }

    [HttpPatch("{userId:int}")]
    public async Task<ActionResult> UpdateUser(int userId, [FromBody] UpdateUserRequest request)
    {
        UpdateUserCommand updateUserCommand = new(userId, request.CourseId, request.Name, request.Email, request.Role);
        await _mediator.Send(updateUserCommand);
        return NoContent();
    }

    [HttpDelete("{userId:int}")]
    public async Task<ActionResult> DeleteUser(int userId)
    {
        DeleteUserByIdCommand deleteUserByIdCommand = new(userId);
        await _mediator.Send(deleteUserByIdCommand);
        return NoContent();
    }
}