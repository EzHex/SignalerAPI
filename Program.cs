using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TowerinoSignaler;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
//Add kestrel configuration to expose port 5235
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 5235);
});
builder.WebHost.UseIIS();

builder.Environment.EnvironmentName = "Production";

var app = builder.Build();

app.UseHttpsRedirection();

app.MapHub<GameHub>("game-hub");

app.Run();