using CidadeInteligente.Core.Entities;
using System.Security.Claims;

namespace CidadeInteligente.Mvc.Extensions;

public static class ClaimsExtensions
{
    extension(ClaimsPrincipal principal)
    {
        public long? UserId
        {
            get
            {
                if (principal.Identity!.IsAuthenticated)
                    return long.Parse(principal.Claims.First(c => c.Type == nameof(User.UserId)).Value);

                return default;
            }
        }
    }
}
