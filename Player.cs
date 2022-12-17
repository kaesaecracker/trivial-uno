using Microsoft.Extensions.Logging;
using TrivialUno.NextTurnStrategies;

namespace TrivialUno;

public class Player
{
    public required string Name { private get; init; }

    public required NextTurnStrategy NextTurnStrategy { private get; init; }

    private readonly ILogger _logger = LoggingManager.Factory.CreateLogger<Player>();
    private readonly List<Card> _hand = new();

    public void DrawCard(Game game) => _hand.Add(game.DrawCard());

    public bool NextTurn(Game game)
    {
        using var logScope = _logger.BeginScope("Turn of {}", Name);
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

