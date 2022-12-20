using TrivialUno.Definitions;

namespace TrivialUno.Strategies.NextTurnParts;

sealed class FirstDrawnFirstPlayed : INextTurnStrategylet
{
    public IReadOnlyList<ICard> FilterOptions(IReadOnlyList<ICard> hand, IReadOnlyList<ICard> remainingOptions, ICard currentTopCard)
        => new List<ICard>(1) { remainingOptions[0] };
}