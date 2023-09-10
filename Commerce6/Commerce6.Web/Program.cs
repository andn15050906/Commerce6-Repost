using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.ResponseCompression;
using Commerce6.Infrastructure;
using Commerce6.Web.Helpers;
using Commerce6.Web.Services;
using Commerce6.Web.Services.JwtService;

var builder = WebApplication.CreateBuilder(args);
AppStart.InitConfig(builder);

// Add services to the container.
builder.Services
    .AddCors(o => o.AddPolicy("Policy", builder =>
        builder.WithOrigins(Configurer.GetCorsOrigins()).AllowCredentials().AllowAnyHeader().AllowAnyMethod()))
    .AddResponseCompression(options =>
     {
         options.EnableForHttps = true;
         options.Providers.Add<GzipCompressionProvider>();
     })
    .AddDistributedMemoryCache()
    .AddSession(options => { options.IdleTimeout = TimeSpan.FromDays(1); });

builder.Services.AddControllers();

string connectionString = Configurer.GetConnectionString();
builder.Services
    .AddDbContext<Context>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped)
    .AddContextServices();
AppStart.ExecuteColdQuery(connectionString);

builder.Services.AddJwt(Configurer.GetJwtOptions());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .UseSerilogRequestLogging()
    .UseHttpsRedirection()
    .UseCors("Policy")
    .UseAuthentication()
    .UseAuthorization()
    .UseSession()
    .UseResponseCompression();

app.MapControllers();
app.Run();

public partial class Program { }            //for integration testing