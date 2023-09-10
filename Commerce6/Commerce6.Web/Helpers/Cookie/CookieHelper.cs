namespace Commerce6.Web.Helpers.Cookie
{
    public class CookieHelper
    {
        private static CookieHelperOptions? _options;

        private CookieHelper() { }

        //similar to singleton
        public static void Init(CookieHelperOptions? options)
        {
            if (_options == null)
                _options = options;
        }

        public static CookieOptions GetOptions() => new()
        {
            SameSite = SameSiteMode.None,
            Secure = _options.Secure,
            Expires = DateTime.Now.AddMinutes(_options.Expires)
        };

        public static CookieOptions GetExpiredOptions() => new()
        {
            SameSite = SameSiteMode.None,
            Secure = _options.Secure,
            Expires = DateTime.Now
        };
    }
}
