using System.Diagnostics;
using System.Security.Claims;

namespace Commerce6.Web.Helpers.Cookie
{
    public static class CookieReader
    {
        /// <summary>
        /// Extension method to get NameIdentifier
        /// </summary>
        public static string? GetUserId(this HttpContext httpContext)
        {
            foreach (Claim claim in httpContext.User.Claims)
                if (claim.Type == ClaimTypes.NameIdentifier)
                    return claim.Value;
            return null;
        }

        /// <summary>
        /// Extension method to set AccessToken
        /// </summary>
        public static void SetAccessToken(this HttpContext httpContext, string accessToken, CookieOptions options)
            => httpContext.Response.Cookies.Append("Bearer", accessToken, options);

        /// <summary>
        /// Extension method to set RefreshToken
        /// </summary>
        public static void SetRefreshToken(this HttpContext httpContext, string refreshToken, CookieOptions options)
            => httpContext.Response.Cookies.Append("Refresh", refreshToken, options);

        /// <summary>
        /// Extension method to get AccessToken
        /// </summary>
        public static string? GetAccessToken(this HttpContext httpContext)
            => httpContext.Request.Cookies["Bearer"];

        /// <summary>
        /// Extension method to get RefreshToken
        /// </summary>
        public static string? GetRefreshToken(this HttpContext httpContext)
            => httpContext.Request.Cookies["Refresh"];
    }
}
