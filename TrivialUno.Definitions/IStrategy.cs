namespace TrivialUno.Definitions;

public interface IStrategy
{
    public ICard? GetNextTurn(IReadOnlyList<ICard> hand, Func<ICard, bool> canBePlayed);
}
