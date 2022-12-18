namespace TrivialUno.Strategies.NextTurnParts;

class FirstDrawnFirstPlayed : INextTurnStrategylet
{
    public IReadOnlyList<Card> FilterOptions(IReadOnlyList<Card> hand, IReadOnlyList<Card> remainingOptions, Card currentTopCard)
        => new List<Card>(1) { remainingOptions[0] };
}
