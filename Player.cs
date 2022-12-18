using TrivialUno.Strategies;

namespace TrivialUno;

class Players : IEnumerable<Player>
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

class Player
{
    private readonly ILogger _logger;
    public Player(ILogger<Player> logger)
    {
        _logger = logger;
    }

    public required string Name { private get; set; }

    public required Strategy NextTurnStrategy { private get; set; }


    private readonly List<Card> _hand = new();

    public void DrawCard(Game game) => _hand.Add(game.DrawCard());

    public bool NextTurn(Game game)
    {
        using var logScope = _logger.BeginScope("{}", this);
        var bestCardToPlay = _hand.Count == 0 ? null : NextTurnStrategy.GetNextTurn(_hand.AsReadOnly(), game.CurrentTopCard);
        if (bestCardToPlay == null)
        {
            DrawCard(game);
            _logger.LogInformation("drawing {}, now {} cards remaining", _hand.Last(), _hand.Count);
            return false;
        }

        if (!bestCardToPlay.CanBePlayedOn(game.CurrentTopCard))
            throw new InvalidOperationException();

        _hand.Remove(bestCardToPlay);
        _logger.LogInformation("playing {}, now {} cards remaining", bestCardToPlay, _hand.Count);
        game.PlayCard(bestCardToPlay);
        return _hand.Count == 0;
    }

    public override string ToString() => $"[Player {Name}]";
}

