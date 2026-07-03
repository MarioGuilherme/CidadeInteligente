using CidadeInteligente.Domain.Enums;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateUser;

public record CreateUserCommand(int CourseId, string Name, string Email, string Password, Role Role) : IRequest<int?> { }
