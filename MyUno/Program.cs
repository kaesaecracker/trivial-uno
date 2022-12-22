using Microsoft.Extensions.Hosting;
using MyUno.ChoiceFilters;
using TrivialUno.DefaultCards;
using TrivialUno.Machinery;

[assembly: CLSCompliant(true)]

namespace MyUno;

public static class Program
{
    public static async Task Main(string[]? args)
    {
        using var host = Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((HostBuilderContext _, IServiceCollection services) => services
                .AddLogging(ConfigureLoggingBuilder)
                .AddSingleton(_ => new Random(0)) // for now, a seed can be picked here
                .AddGame()
            )
            .Build();

        await host.RunAsync().ConfigureAwait(false);
    }
    internal static IServiceCollection AddGame(this IServiceCollection services) => services
        .AddMachinery()
        .AddCards(builder => builder
            .AddDefaultCards()
            .AddCardType<BlackReverseCard>()
        )
        .AddStrategies(builder => builder
            .AddFilteredStrategy("DuplicatesFirst", new Func<IServiceProvider, ICardChoiceFilter>[] {
                sp => ActivatorUtilities.CreateInstance<DuplicateCardTypes>(sp),
                sp => ActivatorUtilities.CreateInstance<PopularColor>(sp),
                sp => ActivatorUtilities.CreateInstance<RandomChoice>(sp),
            })
            .AddFilteredStrategy("FiFo", new Func<IServiceProvider, ICardChoiceFilter>[] {
                sp => ActivatorUtilities.CreateInstance<RandomChoice>(sp),
            })
        )
        .AddHostedService<TestGameHostedService>();

    internal static void ConfigureLoggingBuilder(ILoggingBuilder builder) => builder
        .SetMinimumLevel(LogLevel.Trace)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("TrivialUno.PlayerTurnOrder", LogLevel.Information)
        .AddFilter("TrivialUno.CardTypeManager", LogLevel.Information)
        .AddFilter("TrivialUno.Game", LogLevel.Information)
        .AddSimpleConsole(opts =>
        {
            opts.IncludeScopes = true;
            opts.SingleLine = true;
        });
}
