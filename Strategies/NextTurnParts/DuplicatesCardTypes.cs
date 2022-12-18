using TrivialUno.CardTypes;

namespace TrivialUno.Strategies.NextTurnParts;

class DuplicatesCardTypes : INextTurnStrategylet
{
    public IReadOnlyList<Card> FilterOptions(IReadOnlyList<Card> hand, IReadOnlyList<Card> remainingOptions, Card currentTopCard)
    {
        var playableCardsByType = new Dictionary<ICardType, List<Card>>();
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
