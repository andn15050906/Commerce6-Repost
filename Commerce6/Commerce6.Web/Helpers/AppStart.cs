using Microsoft.EntityFrameworkCore;
using Serilog;
using Commerce6.Infrastructure;
using Commerce6.Web.Helpers.Cookie;

namespace Commerce6.Web.Helpers
{
    public class AppStart
    {
        public static void InitConfig(WebApplicationBuilder builder)
        {
            ConfigLogger(builder);
            Configurer.Init(builder.Configuration);
            CookieHelper.Init(Configurer.GetCookieConfigOptions());
        }

        private static void ConfigLogger(WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(
                    "logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileTimeLimit: TimeSpan.FromDays(7),
                    fileSizeLimitBytes: 536870912)                  // 512 MB
                .CreateLogger();

            Log.Information("__Starting web host");

            builder.Host.UseSerilog();
        }

        public static void ExecuteColdQuery(string connectionString)
        {
            using var context = new Context(new DbContextOptionsBuilder<Context>().UseSqlServer(connectionString).Options);
            context.Products.FirstOrDefault();
        }
    }
}
