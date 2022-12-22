using TrivialUno.Definitions;

namespace TrivialUno;

record struct CardTypeInfo(ICardType CardType, int CardsPerDeck);

sealed class CardTypeManager
{
    private readonly ILogger<CardTypeManager> _logger;

    private readonly IReadOnlyList<CardTypeInfo> _typeInfos;
    private readonly Func<IEnumerable<ICard>, Deck> _deckConstructor;

    public CardTypeManager(
        ILogger<CardTypeManager> logger,
        IReadOnlyList<CardTypeInfo> types,
        Func<IEnumerable<ICard>, Deck> deckConstructor)
    {
        _logger = logger;
        _typeInfos = types;
        _deckConstructor = deckConstructor;
    }

    private IEnumerable<ICard> GenerateCards()
    {
        foreach (var typeInfo in _typeInfos)
        {
            _logger.LogDebug("Generating {} cards for type [{}]", typeInfo.CardsPerDeck, typeInfo.CardType.Name);
            for (int count = 0; count <= typeInfo.CardsPerDeck; count++)
                yield return new Card { CardType = typeInfo.CardType };
        }
    }

    public Deck GenerateNewDeck() => _deckConstructor(GenerateCards());
}
