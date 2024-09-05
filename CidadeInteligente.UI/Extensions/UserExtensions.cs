using System.Security.Claims;

namespace CidadeInteligente.UI.Extensions;

public static class UserExtensions {
    public static long UserId(this ClaimsPrincipal principal) => long.Parse(principal.Claims.First(c => c.Type == "UserId").Value);
}