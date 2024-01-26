using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SoccerManager.Controllers;
using SoccerManager.Models;
using SoccerManager.Validators;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((_, services) =>
{
    services.AddSingleton<IPlayerController, PlayerController>();
    services.AddSingleton<AbstractValidator<Player>, CreateValidator>();
});
builder.Build().Run();

