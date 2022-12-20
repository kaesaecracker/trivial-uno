using TrivialUno.Definitions;

namespace TrivialUno.Strategies.NextTurnParts;

sealed class RandomChoice : INextTurnStrategylet
{
    private readonly Random _random;
    public RandomChoice(Random rand)
    {
        _random = rand;
    }

    public IReadOnlyList<ICard> FilterOptions(IReadOnlyList<ICard> hand, IReadOnlyList<ICard> remainingOptions, ICard currentTopCard)
        => new List<ICard>(1) { remainingOptions[_random.Next(remainingOptions.Count)] };
}
