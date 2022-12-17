using System.Collections;
using Microsoft.Extensions.Logging;
using TrivialUno.CardEffects;
using TrivialUno.CardTypes;

namespace TrivialUno;

class PlayerTurnOder : IEnumerator<Player>
{
    public required Player[] Players { private get; init; }

    private int _currentIndex = 0;
    private int _direction = 1;
    private readonly ILogger _logger = LoggingManager.Factory.CreateLogger<PlayerTurnOder>();

    public Player Current => Players[_currentIndex];

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
        var oldIndex = _currentIndex;
        _currentIndex = GetNextIndex();
        _logger.LogDebug("MoveNext moving from {} to {} with direction {}", oldIndex, _currentIndex, _direction);
        return true;
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void ReverseOrder()
    {
        _direction = -_direction;
    }

    public Player PeekNext() => Players[GetNextIndex()];

    private int GetNextIndex() => (_currentIndex + _direction) % Players.Length;
}

public class Game
{
    private const uint StartingCards = 3;
    private static readonly Random _random = new();

    public required Player[] Players { private get; init; }

    public Card CurrentTopCard => _playStack.Peek();

    public bool Ended { get; private set; } = false;

    private readonly Stack<Card> _drawStack = CardTypeManager.Singleton.GenerateDrawStack();
    private readonly Stack<Card> _playStack = new();
    private readonly ILogger _logger = LoggingManager.Factory.CreateLogger<Game>();
    private PlayerTurnOder? _playerTurnOder;

    public void SetupPhase()
    {
        using var _ = _logger.BeginScope("setup phase");

        _playStack.Push(_drawStack.Pop());
        _logger.LogInformation("First Card: {}", _playStack.Peek());

        for (int i = 0; i < StartingCards; i++)
        {
            foreach (var player in Players)
                player.DrawCard(this);
        }
        _logger.LogInformation("All players drew {} cards", StartingCards);

        _playerTurnOder = new PlayerTurnOder { Players = Players };
        _logger.LogInformation("Order established, first player is {}", _playerTurnOder.Current);
    }

    public Card DrawCard()
    {
        if (_drawStack.TryPop(out var card))
            return card;

        _logger.LogInformation("Shuffling deck");
        var topCard = _playStack.Pop();
        var cardsToShuffle = _playStack.ToArray();
        _playStack.Clear();
        _playStack.Push(topCard);
        _random.Shuffle(cardsToShuffle);
        _drawStack.PushRange(cardsToShuffle);
        return _drawStack.Pop();
    }

    public void PlayCard(Card card)
    {
        _playStack.Push(card);

        if (card is IEffectCardType effectCardType)
        {
            foreach (var effect in effectCardType.CardEffects)
            {
                HandleEffect(effect);
            }
        }
    }

    private void HandleEffect(ICardEffect effect)
    {
        switch (effect)
        {
            case ForceNextPlayerDrawEffect draw:
                HandleDrawCardEffect(draw);
                break;
            case ChooseColorEffect color:
                HandleChooseColorEffect(color);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private bool HandleChooseColorEffect(ChooseColorEffect color)
    {
        throw new NotImplementedException();
    }

    private void HandleDrawCardEffect(ForceNextPlayerDrawEffect draw)
    {
        var nextPlayer = _playerTurnOder!.PeekNext();
        for (int i = 0; i < draw.CardsToDraw; i++)
            nextPlayer.DrawCard(this);

        _logger.LogInformation("the played card {} forced {} to draw {} cards", draw, nextPlayer, draw.CardsToDraw);

    }

    internal void Advance()
    {
        if (Ended) throw new InvalidOperationException();
        var player = _playerTurnOder?.Current ?? throw new InvalidOperationException();

        if (player.NextTurn(this))
        {
            Ended = true;
            _logger.LogInformation("{} wins", player);
            return;
        }

        _playerTurnOder.MoveNext();
    }
}

