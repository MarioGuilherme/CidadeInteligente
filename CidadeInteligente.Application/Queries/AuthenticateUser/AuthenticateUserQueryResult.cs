using System.Security.Claims;

namespace CidadeInteligente.Application.Queries.AuthenticateUser;

public record AuthenticateUserQueryResult(IEnumerable<Claim> Claims);
