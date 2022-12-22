using TrivialUno.Definitions;

namespace TrivialUno.Strategies.NextTurnParts;

sealed class PopularColor : INextTurnPart
{
    public IReadOnlyList<ICard> FilterOptions(IReadOnlyList<ICard> hand, IReadOnlyList<ICard> remainingOptions)
    {
        var cardsByColor = new Dictionary<CardColor, List<ICard>>();
        foreach (var card in hand)
        {
            if (card.CardType is not IColoredCardType colored)
                continue;
            if (!cardsByColor.ContainsKey(colored.Color))
                cardsByColor[colored.Color] = new();
            cardsByColor[colored.Color].Add(card);
        }

        var maxColorCount = cardsByColor.Max(pair => pair.Value.Count);
        return remainingOptions
            .Where(c => c.CardType is not IColoredCardType colored || cardsByColor[colored.Color].Count == maxColorCount)
            .ToList()
            .AsReadOnly();
    }
}
