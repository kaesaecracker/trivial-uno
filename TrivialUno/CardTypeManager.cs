using TrivialUno.Definitions;

namespace TrivialUno;

sealed class CardTypeManager
{
    private readonly ILogger<CardTypeManager> _logger;

    internal required IReadOnlyList<ICardType> Types { get; init; }

    internal required Dictionary<ICardType, int> CardsPerType { get; init; }

    internal CardTypeManager(ILogger<CardTypeManager> logger)
    {
        _logger = logger;
    }

    private IEnumerable<ICard> GenerateCardsForType(ICardType cardType)
    {
        var maxCount = CardsPerType[cardType];
        _logger.LogDebug("Generating {} cards for type [{}]", maxCount, cardType.Name);
        for (int count = 0; count <= maxCount; count++)
            yield return new Card { CardType = cardType };
    }

    public Deck GenerateNewDeck() => new(Types.SelectMany(GenerateCardsForType));
}
