using CidadeInteligente.Domain.Entities;
using System.Security.Claims;

namespace CidadeInteligente.Mvc.Extensions;

public static class ClaimsExtensions
{
    extension(ClaimsPrincipal principal)
    {
        public int? UserId
        {
            get
            {
                if (principal.Identity!.IsAuthenticated)
                    return int.Parse(principal.Claims.First(c => c.Type is nameof(User.UserId)).Value);

                return default;
            }
        }
    }
}
