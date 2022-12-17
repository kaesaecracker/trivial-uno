namespace TrivialUno.NextTurnStrategies;

class PlayableStrategylet : INextTurnStrategylet
{
    public static PlayableStrategylet Singleton { get; } = new();
    private PlayableStrategylet() { }

    public IReadOnlyList<Card> FilterOptions(IReadOnlyList<Card> hand, IReadOnlyList<Card> remainingOptions, Card currentTopCard)
    {
        return remainingOptions.Where(c => c.CanBePlayedOn(currentTopCard)).ToList().AsReadOnly();
    }
}
