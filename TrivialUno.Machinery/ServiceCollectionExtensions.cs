using TrivialUno.Definitions.Builders;

namespace TrivialUno.Machinery;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCards(this IServiceCollection services, Action<ICardTypeManagerBuilder> buildAction) => services
        .AddSingleton(services =>
        {
            var ctmBuilder = new CardTypeManagerBuilder(services);
            buildAction(ctmBuilder);
            return ctmBuilder.Build();
        });

    public static IServiceCollection AddStrategies(this IServiceCollection services, Action<IStrategyManagerBuilder> buildAction) => services
        .AddSingleton(services =>
        {
            var ctmBuilder = new StrategyManagerBuilder(services);
            buildAction(ctmBuilder);
            return ctmBuilder.Build();
        });

    public static IServiceCollection AddMachinery(this IServiceCollection services) => services
        .AddSingleton<GameRules>()
        .AddScoped<PlayerTurnOrder>()
        .AddScoped<IGame, Game>()
        .AddScoped<IPlayers, Players>()
        .AddTransient<IPlayer, Player>();
}
