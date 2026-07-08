using CidadeInteligente.Domain.Enums;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateUser;

public record CreateUserCommand(int CourseId, string Name, string Email, string Password, string ConfirmPassword, Role Role) : IRequest<int?> { }
