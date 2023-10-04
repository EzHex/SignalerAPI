using System.Net;
using TowerinoSignaler;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapHub<GameHub>("game-hub");

app.Run();