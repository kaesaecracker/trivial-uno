using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrivialUno.CardEffects;
using TrivialUno.CardTypes;

namespace TrivialUno;

public class Game
{
    private readonly Random _random;
    private readonly Players _players;
    private readonly ILogger<Game> _logger;
    private readonly Stack<Card> _drawStack = CardTypeManager.Singleton.GenerateDrawStack();
    private readonly Stack<Card> _playStack = new();
    private readonly PlayerTurnOrder _playerTurnOder;
    public Game(ILogger<Game> logger, Random rand, Players players, PlayerTurnOrder turnOder)
    {
        _logger = logger;
        _random = rand;
        _players = players;
        _playerTurnOder = turnOder;
    }

    private const uint StartingCards = 3;

    public Card CurrentTopCard => _playStack.Peek();

    public bool Ended { get; private set; } = false;

    public void SetupPhase()
    {
        using var _ = _logger.BeginScope("setup phase");

        _playStack.Push(_drawStack.Pop());
        _logger.LogInformation("First Card: {}", _playStack.Peek());

        for (int i = 0; i < StartingCards; i++)
        {
            foreach (var player in _players)
                player.DrawCard(this);
        }
        _logger.LogInformation("All players drew {} cards", StartingCards);
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

