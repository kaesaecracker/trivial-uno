namespace TrivialUno;

static class Startup
{
    internal static IServiceCollection AddGame(this IServiceCollection services) => services
        .AddHostedService<TestGameHostedService>()
        .AddSingleton<CardTypes.CardTypeManager>()
        .AddSingleton<Strategies.FiFoStrategy>()
        .AddSingleton<Strategies.DuplicatesFirstStrategy>()
        .AddSingleton<Strategies.NextTurnParts.DuplicatesCardTypes>()
        .AddSingleton<Strategies.NextTurnParts.FirstDrawnFirstPlayed>()
        .AddSingleton<Strategies.NextTurnParts.Playable>()
        .AddSingleton<Strategies.NextTurnParts.PopularColor>()
        .AddScoped<PlayerTurnOrder>()
        .AddScoped<Game>()
        .AddScoped<Players>()
        .AddTransient<Player>();

    internal static void ConfigureLoggingBuilder(ILoggingBuilder builder) => builder
        .SetMinimumLevel(LogLevel.Trace)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("TrivialUno.PlayerTurnOrder", LogLevel.Information)
        .AddSimpleConsole(opts =>
        {
            opts.IncludeScopes = true;
            opts.SingleLine = true;
        });
}