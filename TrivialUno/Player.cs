using TrivialUno.Definitions;
using TrivialUno.Strategies;

namespace TrivialUno;

sealed class Players : IEnumerable<Player>
{
    private readonly List<Player> _allPlayers = new();

    public void Add(Player p)
    {
        _allPlayers.Add(p);
    }

    public IEnumerator<Player> GetEnumerator() => _allPlayers.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Player this[int key]
    {
        get => _allPlayers[key];
    }

    public int Count => _allPlayers.Count;
}

sealed class Player : IReadOnlyPlayer, IPlayer
{
    private readonly ILogger _logger;

    public Player(ILogger<Player> logger)
    {
        _logger = logger;
    }

    public required string Name { private get; set; }

    public required Strategy PlayCardStrategy { private get; set; }

    public int CardsLeft => _hand.Count;

    private readonly List<ICard> _hand = new();

    public void PickupCard(ICard card)
    {
        _hand.Add(card);
        _logger.LogInformation("{} picks up {}, now has {} cards", this, card, _hand.Count);
    }

    public ICard? ChooseCardToPlay(Func<ICard, bool> canBePlayed)
    {
        var card = _hand.Count == 0 ? null : PlayCardStrategy.GetNextTurn(_hand.AsReadOnly(), canBePlayed);
        if (card != null && !_hand.Remove(card))
            throw new IllegalMoveException($"The card choosen by {PlayCardStrategy} is not one of the cards in hands of {this}");
        return card;
    }

    public ICard ChooseCardToDiscard()
    {
        _logger.LogWarning("ChooseCardToDiscard does not have strategies");
        var card = _hand[0];
        _hand.RemoveAt(0);
        return card;
    }

    public CardColor ChooseColor()
    {
        _logger.LogWarning("ChooseFavoriteColor does not have strategies");
        var cardByColor = _hand.Where(c => c.CardType is IColoredCardType)
            .Select(c => (IColoredCardType)c.CardType)
            .GroupBy(c => c.Color)
            .ToList();

        if (cardByColor.Count == 0)
            return CardColor.Yellow;

        var maxCardsForColor = cardByColor.Max(group => group.Count());
        return cardByColor.Where(group => group.Count() == maxCardsForColor)
            .Select(group => group.Key)
            .First();
    }

    public override string ToString() => $"[Player {Name}]";

}
