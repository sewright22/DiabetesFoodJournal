using WebApi.Features;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;
using WebApi;

var builder = WebApplication.CreateBuilder();
builder.ConfigureServices();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();

app.Run();