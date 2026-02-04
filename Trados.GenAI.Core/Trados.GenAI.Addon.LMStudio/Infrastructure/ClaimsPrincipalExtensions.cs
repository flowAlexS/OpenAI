using System.Security.Claims;

namespace Trados.GenAI.Addon.LMStudio.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetTenantId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("aid");
        }
    }
}
