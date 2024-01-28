using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SoccerManager.Controller;
using SoccerManager.Models;
using SoccerManager.Services;
using SoccerManager.Validators;

var builder = Host.CreateDefaultBuilder(args);
var host = builder.ConfigureServices((_, services) =>
{
    services.AddSingleton<IPlayerService, PlayerService>();
    services.AddSingleton<AbstractValidator<Player>, CreateValidator>();
    services.AddSingleton<IPlayerController, PlayerController>();
}).Build();

using var app = host.Services.CreateScope();
app.ServiceProvider.GetRequiredService<IPlayerController>().Run();