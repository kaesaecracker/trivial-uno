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

sealed class Player : IPlayer
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
        _logger.LogInformation("{} draws {}", this, card);
        _hand.Add(card);
    }

    public ICard? ChooseCardToPlay(Func<ICard, bool> canBePlayed)
    {
        var card = _hand.Count == 0 ? null : PlayCardStrategy.GetNextTurn(_hand.AsReadOnly(), canBePlayed);
        if (card != null)
            _hand.Remove(card);
        return card;
    }

    public ICard ChooseCardToDiscard()
    {
        // TODO: implement strategies
        var card = _hand[0];
        _hand.RemoveAt(0);
        return card;
    }

    public override string ToString() => $"[Player {Name}]";
}

