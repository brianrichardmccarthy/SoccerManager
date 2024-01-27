using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SoccerManager.Models;
using SoccerManager.Services;
using SoccerManager.Validators;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((_, services) =>
{
    services.AddSingleton<IPlayerService, PlayerService>();
    services.AddSingleton<AbstractValidator<Player>, CreateValidator>();
});
builder.ConfigureLogging(logger =>
{
    logger.AddConsole();
});

builder.Build().Run();
