using System.Net;
using TowerinoSignaler;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
//Add use urls from all devices
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 7779);
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapHub<GameHub>("game-hub");

app.Run();