using TrivialUno.CardEffects;
using TrivialUno.Definitions;

namespace TrivialUno;

sealed class Game
{
    private readonly Random _random;
    private readonly Players _players;
    private readonly ILogger<Game> _logger;
    private readonly GameRules _rules;
    private readonly PlayerTurnOrder _playerTurnOder;
    private readonly Deck _deck;
    private ICard? lastPlayedCard;

    public Game(ILogger<Game> logger, Random rand, Players players, PlayerTurnOrder turnOder, CardTypeManager cardTypeManager, GameRules rules)
    {
        _logger = logger;
        _random = rand;
        _players = players;
        _playerTurnOder = turnOder;
        _deck = cardTypeManager.GenerateNewDeck();
        _rules = rules;
    }

    private void SetupPhase()
    {
        using var scope = _logger.BeginScope("setup phase");
        _deck.Shuffle(_random);

        _logger.LogInformation("All players will now draw {} cards", _rules.StartingCardsPerPlayer);
        for (int i = 0; i < _rules.StartingCardsPerPlayer; i++)
        {
            foreach (var player in _players)
                player.DrawCard(TakeFromDrawStack());
        }

        LastPlayedCard = TakeFromDrawStack();
        PlayCard(LastPlayedCard);
        _logger.LogInformation("First Card: {}", LastPlayedCard);
    }

    private ICard TakeFromDrawStack()
    {
        if (_deck.CardsRemaining == 0)
            _deck.Shuffle(_random);
        return _deck.Draw();
    }

    public ICard LastPlayedCard
    {
        get => lastPlayedCard ?? throw new InvalidOperationException();
        private set => lastPlayedCard = value;
    }

    public async Task Run(CancellationToken cancellationToken)
    {
        SetupPhase();
        while (!cancellationToken.IsCancellationRequested && Advance())
        {
            await Task.Delay(0, cancellationToken).ConfigureAwait(false);
        }
    }

    private void PlayCard(ICard card)
    {
        LastPlayedCard = card;
        //if (card.CardType is IEffectCardType effectCardType)
        //{
        //    using var scope = _logger.BeginScope("effects of {Card}", card);
        //    foreach (var effect in effectCardType.CardEffects)
        //        HandleEffect(effect);
        //}
        _deck.Discard(card);
    }

    private void HandleEffect(ICardEffect effect)
    {
        switch (effect)
        {
            //case ForceNextPlayerDrawEffect draw:
            //    _logger.LogInformation("the played card forces {} to draw {} cards", _playerTurnOder.Next, draw.CardsToDraw);
            //    for (int i = 0; i < draw.CardsToDraw; i++)
            //        _playerTurnOder.Next.DrawCard(TakeFromDrawStack());
            //    break;
            case ReverseEffect:
                _playerTurnOder.ReverseOrder();
                break;
            default:
                throw new NotImplementedException($"Effect of type {effect.GetType().Name} not implemeneted");
        }
    }

    internal bool Advance()
    {
        var player = _playerTurnOder.Current;
        _logger.LogDebug("Starting turn of {}", player);

        var playerChoosenCard = player.ChooseCardToPlay(LastPlayedCard);
        if (playerChoosenCard == null)
        {
            var drawCard = TakeFromDrawStack();
            player.DrawCard(drawCard);
            _playerTurnOder.EndTurnOfCurrentPlayer();
            return true;
        }

        if (!playerChoosenCard.CanBePlayedOn(LastPlayedCard))
            throw new IllegalMoveException($"{player} tried to play {playerChoosenCard} onto {LastPlayedCard}!");

        PlayCard(playerChoosenCard);
        _logger.LogInformation("{Player} plays {Card}", player, playerChoosenCard);

        if (player.CardsLeft == 0)
        {
            _logger.LogInformation("{} wins", player);
            return false;
        }

        if (player.CardsLeft > _rules.MaxCardsInHand)
        {
            _logger.LogInformation("{} has to discard cards because he has {} out of {}", player, player.CardsLeft, _rules.MaxCardsInHand);
            while (player.CardsLeft > _rules.MaxCardsInHand)
            {
                _deck.Discard(player.ChooseCardToDiscard());
            }
        }

        _playerTurnOder.EndTurnOfCurrentPlayer();
        return true;
    }
}
