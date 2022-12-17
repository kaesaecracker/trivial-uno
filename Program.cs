using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrivialUno.Strategies;
using TrivialUno.CardTypes;
using Microsoft.Extensions.Logging.Console;

namespace TrivialUno;

public class Program
{
    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
    {
        _ = services
            .AddLogging(ConfigureLoggingBuilder)
            .AddTransient<NextTurnStrategy>()
            .AddTransient<Player>()
            .AddSingleton<CardTypeManager>()
            .AddSingleton<StrategyManager>()
            .AddScoped<PlayerTurnOrder>()
            .AddScoped(_ => new Random(0))
            .AddScoped<Game>()
            .AddScoped<Players>();
    }

    private static void ConfigureLoggingBuilder(ILoggingBuilder builder)
    {
        builder.SetMinimumLevel(LogLevel.Trace)
            .AddFilter("System", LogLevel.Warning)
            .AddFilter("TrivialUno.PlayerTurnOrder", LogLevel.Information)
            .AddSimpleConsole(opts =>
            {
                opts.IncludeScopes = true;
                opts.SingleLine = true;
            });
    }

    public static async Task Main(string[]? args)
    {
        using IHost host = Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices)
            .Build();

        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        logger.LogTrace("Main()");

        var gameScope = host.Services.CreateScope();
        var gameServiceProvider = gameScope.ServiceProvider;

        var strategyMgr = gameServiceProvider.GetRequiredService<StrategyManager>();
        var players = gameServiceProvider.GetRequiredService<Players>();
        var game = gameServiceProvider.GetRequiredService<Game>();

        var p1 = gameServiceProvider.GetRequiredService<Player>();
        p1.Name = "P1";
        p1.NextTurnStrategy = strategyMgr.GetNextTurnStrategy("FiFo");
        players.Add(p1);

        var p2 = gameServiceProvider.GetRequiredService<Player>();
        p2.Name = "P2";
        p2.NextTurnStrategy = strategyMgr.GetNextTurnStrategy("Duplicates");
        players.Add(p2);

        var p3 = gameServiceProvider.GetRequiredService<Player>();
        p3.Name = "P3";
        p3.NextTurnStrategy = strategyMgr.GetNextTurnStrategy("Complex");
        players.Add(p3);

        game.SetupPhase();
        while (!game.Ended)
            game.Advance();

        await host.RunAsync();
    }
}