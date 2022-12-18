namespace TrivialUno.Strategies.NextTurnParts;

sealed class Playable : INextTurnStrategylet
{
    public IReadOnlyList<Card> FilterOptions(IReadOnlyList<Card> hand, IReadOnlyList<Card> remainingOptions, Card currentTopCard)
    {
        return remainingOptions.Where(c => c.CanBePlayedOn(currentTopCard)).ToList().AsReadOnly();
    }
}
