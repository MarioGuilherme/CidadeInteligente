using System.Security.Claims;

namespace CidadeInteligente.Application.Commands.LoginUser;

public record LoginUserCommandResult(ClaimsPrincipal ClaimsPrincipal);
