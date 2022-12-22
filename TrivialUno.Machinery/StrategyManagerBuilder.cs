using TrivialUno.Definitions.Builders;

namespace TrivialUno.Machinery;

internal sealed class StrategyManagerBuilder : IStrategyManagerBuilder
{
    private readonly IServiceProvider _services;

    private readonly Dictionary<string, IStrategy> _strategies = new();

    public StrategyManagerBuilder(IServiceProvider services)
    {
        _services = services;
    }

    public IStrategyManagerBuilder AddStrategy(string name, Func<IServiceProvider, IStrategy> strategy)
    {
        _strategies.Add(name, strategy(_services));
        return this;
    }

    public IStrategyManagerBuilder AddFilteredStrategy(string name, IEnumerable<Func<IServiceProvider, ICardChoiceFilter>> filterConstructors)
    {
        _strategies.Add(name, ActivatorUtilities.CreateInstance<FilteredStrategy>(_services, filterConstructors.Select(f => f(_services))));
        return this;
    }

    public IStrategyManager Build() => new StrategyManager(_strategies);
}
