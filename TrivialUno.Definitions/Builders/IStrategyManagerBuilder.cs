namespace TrivialUno.Definitions.Builders;

public interface IStrategyManagerBuilder
{
    public IStrategyManagerBuilder AddStrategy(string name, Func<IServiceProvider, IStrategy> contructor);

    public IStrategyManagerBuilder AddFilteredStrategy(string name, IEnumerable<Func<IServiceProvider, ICardChoiceFilter>> filterConstructors);
}
