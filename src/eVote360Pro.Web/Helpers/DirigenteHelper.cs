using System.Security.Claims;

namespace eVote360Pro.Web.Helpers
{
    public static class DirigenteHelper
    {
        public static int? GetPartidoPoliticoId(ClaimsPrincipal user)
        {
            var claim = user.FindFirst("PartidoPoliticoId");
            return claim != null && int.TryParse(claim.Value, out var id) ? id : null;
        }

        public static int? GetUsuarioId(ClaimsPrincipal user)
        {
            var claim = user.FindFirst("UsuarioId");
            return claim != null && int.TryParse(claim.Value, out var id) ? id : null;
        }
    }
}
