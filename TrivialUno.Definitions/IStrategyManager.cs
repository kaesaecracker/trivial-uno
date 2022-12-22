namespace TrivialUno.Definitions;

public interface IStrategyManager
{
    public IStrategy GetStrategy(string name);
}
