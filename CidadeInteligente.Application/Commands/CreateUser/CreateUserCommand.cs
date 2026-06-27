using CidadeInteligente.Core.Enums;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateUser;

public record CreateUserCommand(long CourseId, string Name, string Email, string Password, Role Role) : IRequest<long> { }
