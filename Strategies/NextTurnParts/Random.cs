namespace TrivialUno.Strategies.NextTurnParts;

sealed class RandomChoice : INextTurnStrategylet
{
    private readonly Random _random;
    public RandomChoice(Random rand)
    {
        _random = rand;
    }

    public IReadOnlyList<Card> FilterOptions(IReadOnlyList<Card> hand, IReadOnlyList<Card> remainingOptions, Card currentTopCard)
        => new List<Card>(1) { remainingOptions[_random.Next(remainingOptions.Count)] };
}
