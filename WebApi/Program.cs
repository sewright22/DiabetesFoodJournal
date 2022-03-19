using WebApi.Features;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;
using WebApi;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder();

Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
          .Enrich.FromLogContext()
          .WriteTo.Console()
          .WriteTo.File("./logs/log-.txt", rollingInterval: RollingInterval.Day)
          .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Host.UseSerilog();

builder.ConfigureServices();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();

app.Run();