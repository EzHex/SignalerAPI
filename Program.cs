using System.Net;
using TowerinoSignaler;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
// Listen on all IP addresses and port 5235
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5235);
});
// Listen on a specific IP address and port 5235
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Parse("158.129.1.136"), 5235);
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapHub<GameHub>("game-hub");

app.Run();