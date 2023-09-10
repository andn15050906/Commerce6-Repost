using Commerce6.Data.Domain.Merchant;

namespace Commerce6.Web.Helpers.Session
{
    public static class SessionExtension
    {
        /// <summary>
        /// Extension method to set ShopId
        /// </summary>
        public static void SetShopId(this HttpContext httpContext, int shopId)
            => httpContext.Session.SetInt32("ShopId", shopId);

        /// <summary>
        /// Extension method to get ShopId
        /// </summary>
        public static int? GetShopId(this HttpContext httpContext)
            => httpContext.Session.GetInt32("ShopId");
    }
}
