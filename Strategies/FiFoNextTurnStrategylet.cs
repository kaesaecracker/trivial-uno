namespace TrivialUno.Strategies;

class FiFoStrategylet : INextTurnStrategylet
{
    public static FiFoStrategylet Singleton { get; } = new();
    private FiFoStrategylet() { }

    public IReadOnlyList<Card> FilterOptions(IReadOnlyList<Card> hand, IReadOnlyList<Card> remainingOptions, Card currentTopCard)
        => new List<Card>(1) { remainingOptions[0] };
}
