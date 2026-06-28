using CidadeInteligente.Application.Commands.CreateUser;
using CidadeInteligente.Application.Commands.DeleteUserById;
using CidadeInteligente.Application.Commands.UpdateUser;
using CidadeInteligente.Application.Queries.GetUserById;
using CidadeInteligente.Application.Queries.GetUsers;
using CidadeInteligente.Core.Enums;
using CidadeInteligente.Mvc.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.Mvc.Controllers;

[Route("api/users")]
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

    [HttpGet("{userId}")]
    public async Task<ActionResult> GetUserById(long userId)
    {
        GetUserByIdQuery getUserByIdQuery = new(userId);
        GetUserByIdQueryResult? getUserByIdQueryResult = await _mediator.Send(getUserByIdQuery);
        return Ok(getUserByIdQueryResult);
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        long? userId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUserById), new { userId }, command);
    }

    [HttpPatch("{userId}")]
    public async Task<ActionResult> UpdateUser(long userId, [FromBody] UpdateUserRequest request)
    {
        UpdateUserCommand updateUserCommand = new(userId, request.CourseId, request.Name, request.Email, request.Role);
        await _mediator.Send(updateUserCommand);
        return NoContent();
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult> DeleteUser(long userId)
    {
        DeleteUserByIdCommand deleteUserByIdCommand = new(userId);
        await _mediator.Send(deleteUserByIdCommand);
        return NoContent();
    }
}