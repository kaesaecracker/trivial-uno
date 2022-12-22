namespace TrivialUno.Machinery;

internal sealed class StrategyManager : IStrategyManager
{
    private readonly Dictionary<string, IStrategy> _strategies;

    public StrategyManager(Dictionary<string, IStrategy> strategies)
    {
        _strategies = strategies;
    }

    public IStrategy GetStrategy(string name) => _strategies[name];
}
