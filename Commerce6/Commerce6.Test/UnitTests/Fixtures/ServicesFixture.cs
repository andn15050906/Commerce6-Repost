using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Commerce6.Infrastructure;
using Commerce6.Web.Services.JwtService;
using Commerce6.Web.Helpers.Cookie;

namespace Commerce6.Test.UnitTests.Fixtures
{
    public class ServicesFixture
    {
        //private readonly ITestOutputHelper _output;
        public Context Context { get; private set; }

        //...
        public JwtOptions JwtOptions { get; private set; }

        public ServicesFixture()
        {
            string dir = Directory.GetCurrentDirectory();
            int end = dir.LastIndexOf(@"Commerce6\");
            string path = dir.Substring(0, end + @"ommerce6\".Length) + @"\Commerce6.Web";
            //output.WriteLine(path);

            IConfigurationRoot root = new ConfigurationBuilder().SetBasePath(path)
                .AddJsonFile("appsettings.json").Build();
            //output.WriteLine(root.GetDebugView());

            string connectionString = root.GetConnectionString(root.GetValue<string>("TargetConnectionString"));
            Context = new Context(new DbContextOptionsBuilder<Context>().UseSqlServer(connectionString).Options);

            //...
            JwtOptions = root.GetSection("JwtOptions").Get<JwtOptions>();
            CookieHelper.Init(root.GetSection("CookieOptions").Get<CookieHelperOptions>());
        }
    }
}
