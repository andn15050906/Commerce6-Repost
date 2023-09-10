using Commerce6.Web.Helpers.Cookie;
using Commerce6.Web.Services.JwtService;

namespace Commerce6.Web.Helpers
{
    public class Configurer
    {
        private static ConfigurationManager? _configuration;

        private Configurer() { }

        //similar to singleton
        public static void Init(ConfigurationManager configuration)
        {
            if (_configuration == null)
                _configuration = configuration;
        }

        public static string GetConnectionString()
        {
            string targetConnectionString = _configuration.GetValue<string>("TargetConnectionString");
            return _configuration.GetConnectionString(targetConnectionString);
        }

        public static CookieHelperOptions GetCookieConfigOptions()
            => _configuration.GetSection("CookieOptions").Get<CookieHelperOptions>();

        public static JwtOptions GetJwtOptions()
            => _configuration.GetSection("JwtOptions").Get<JwtOptions>();

        public static string[] GetCorsOrigins()
            => _configuration.GetSection("CORS").Get<string[]>();
    }
}
