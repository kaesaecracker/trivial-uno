using TrivialUno.Definitions;

namespace TrivialUno.Strategies.NextTurnParts;

sealed class DuplicatesCardTypes : INextTurnStrategylet
{
    public IReadOnlyList<ICard> FilterOptions(IReadOnlyList<ICard> hand, IReadOnlyList<ICard> remainingOptions)
    {
        var playableCardsByType = new Dictionary<ICardType, List<ICard>>();
        foreach (var option in remainingOptions)
        {
            if (!playableCardsByType.TryGetValue(option.CardType, out var value))
            {
                playableCardsByType[option.CardType] = new() { option };
                continue;
            }

            playableCardsByType[option.CardType].Add(option);
        }

        var maxTypeOccurrences = playableCardsByType.Max(pair => pair.Value.Count);
        return remainingOptions.Where(c => playableCardsByType[c.CardType].Count == maxTypeOccurrences).ToList().AsReadOnly();
    }
}
