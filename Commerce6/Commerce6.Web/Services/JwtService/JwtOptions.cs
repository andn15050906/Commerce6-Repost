namespace Commerce6.Web.Services.JwtService
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public string Secret { get; set; } = "";
        public int Lifetime { get; set; }               //in minutes
    }
}
