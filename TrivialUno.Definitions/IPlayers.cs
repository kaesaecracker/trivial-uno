namespace TrivialUno.Definitions;

public interface IPlayers : IEnumerable<IPlayer>
{
    int Count { get; }

    public void Add(string name, IStrategy strat);

    public IPlayer this[int key] { get; }
}
