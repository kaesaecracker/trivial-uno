using TrivialUno.CardTypes.Default;
using TrivialUno.Definitions;

namespace TrivialUno;

static class Startup
{
    internal static IServiceCollection AddGame(this IServiceCollection services) => services
        .AddHostedService<TestGameHostedService>()
        .AddCardTypeManager(ConfigureCardTypes)
        .AddSingleton<Strategies.FiFoStrategy>()
        .AddSingleton<Strategies.DuplicatesFirstStrategy>()
        .AddSingleton<Strategies.NextTurnParts.DuplicatesCardTypes>()
        .AddSingleton<Strategies.NextTurnParts.FirstDrawnFirstPlayed>()
        .AddSingleton<Strategies.NextTurnParts.RandomChoice>()
        .AddSingleton<Strategies.NextTurnParts.PopularColor>()
        .AddSingleton<GameRules>()
        .AddScoped<PlayerTurnOrder>()
        .AddScoped<Game>()
        .AddScoped<Players>()
        .AddTransient<Player>();

    internal static void ConfigureLoggingBuilder(ILoggingBuilder builder) => builder
        .SetMinimumLevel(LogLevel.Trace)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("TrivialUno.PlayerTurnOrder", LogLevel.Information)
        .AddFilter("TrivialUno.Game", LogLevel.Information)
        .AddSimpleConsole(opts =>
        {
            opts.IncludeScopes = true;
            opts.SingleLine = true;
        });

    internal static void ConfigureCardTypes(ICardTypeManagerBuilder builder) => builder
        .AddCardType<ColoredDrawCardType>()
        .AddCardType<ColoredReverseCard>()
        .AddCardType<ColoredZeroCardType>()
        .AddCardType<ColoredNumberCardType>()
        .AddCardType<BlackDrawCardType>()
        .AddCardType<BlackColorChooseCardType>();
}
