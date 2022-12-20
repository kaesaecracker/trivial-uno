using TrivialUno.Definitions;

namespace TrivialUno.Strategies.NextTurnParts;

sealed class Playable : INextTurnStrategylet
{
    public IReadOnlyList<ICard> FilterOptions(IReadOnlyList<ICard> hand, IReadOnlyList<ICard> remainingOptions, ICard currentTopCard)
    {
        ArgumentNullException.ThrowIfNull(currentTopCard);

        return remainingOptions
            .Where(c => c.CanBePlayedOn(currentTopCard))
            .ToList()
            .AsReadOnly();
    }
}
