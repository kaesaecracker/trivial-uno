using Microsoft.Extensions.Hosting;
using MyUno.ChoiceFilters;
using MyUno.CustomCards;
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
            .ConfigureServices(ConfigureServices)
            .Build();

        await host.RunAsync().ConfigureAwait(false);
    }

    public static void ConfigureServices(HostBuilderContext _, IServiceCollection services) => services
        .AddLogging(builder => builder
            .SetMinimumLevel(LogLevel.Trace)
            .AddFilter("System", LogLevel.Warning)
            .AddFilter("TrivialUno.Machinery", LogLevel.Debug)
            .AddFilter("TrivialUno.Machinery.PlayerTurnOrder", LogLevel.Information)
            .AddSimpleConsole(opts =>
            {
                opts.IncludeScopes = true;
                opts.SingleLine = true;
            })
        )
        .AddSingleton(_ => new Random(0)) // for now, a seed can be picked here
        .AddMachinery()
        .AddCards(builder => builder
            .AddDefaultCards()
            .AddCardType<BlackReverseCard>()
            .AddCardType<EveryoneDrawsCard>()
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
}
