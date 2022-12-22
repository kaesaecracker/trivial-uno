namespace TrivialUno.Machinery;

sealed class Game : IGame
{
    private readonly Random _random;
    private readonly IPlayers _players;
    private readonly ILogger<Game> _logger;
    private readonly GameRules _rules;
    private readonly IDeck _deck;
    private readonly List<IPlayabilityFilter> _playabilityFiltersForNextTurn = new();
    private readonly PlayerTurnOrder _playerTurnOrder;

    private ICard? _lastPlayedCard;
    private int _round;

    public Game(ILogger<Game> logger, Random rand, IPlayers players, PlayerTurnOrder turnOder, ICardTypeManager cardTypeManager, GameRules rules)
    {
        _logger = logger;
        _random = new Random(rand.Next());
        _players = players;
        _playerTurnOrder = turnOder;
        _deck = cardTypeManager.GenerateNewDeck();
        _rules = rules;
    }

    private ICard LastPlayedCard
    {
        get => _lastPlayedCard ?? throw new InvalidOperationException("last played card can only be read after the first card has been played");
        set => _lastPlayedCard = value;
    }

    public IPlayer CurrentPlayer => _playerTurnOrder.Current;

    IReadOnlyPlayer IReadOnlyGame.CurrentPlayer => CurrentPlayer;

    public IPlayer NextPlayer => _playerTurnOrder.Next;

    IReadOnlyPlayer IReadOnlyGame.NextPlayer => NextPlayer;

    private void SetupPhase()
    {
        using var scope = _logger.BeginScope("setup phase");
        _deck.Shuffle();

        _logger.LogInformation("All players will now draw {} cards", _rules.StartingCardsPerPlayer);
        for (int i = 0; i < _rules.StartingCardsPerPlayer; i++)
        {
            foreach (var player in _players)
                GiveCardTo(player);
        }

        LastPlayedCard = TakeFromDrawStack();
        PlayCard(LastPlayedCard);
        _logger.LogInformation("First Card: {}", LastPlayedCard);
    }

    private ICard TakeFromDrawStack()
    {
        if (_deck.CardsRemaining == 0)
            _deck.Shuffle();
        return _deck.Draw();
    }

    public async Task Run(CancellationToken cancellationToken)
    {
        SetupPhase();
        while (!cancellationToken.IsCancellationRequested && Advance())
            await Task.Delay(0, cancellationToken).ConfigureAwait(false);

        if (cancellationToken.IsCancellationRequested)
            _logger.LogWarning("Game has been aborted");
    }

    private void PlayCard(ICard card)
    {
        using var scope = _logger.BeginScope("playing of {Card}", card);
        LastPlayedCard = card;
        foreach (var effect in card.GetEffects())
            effect.Apply(this);
        _deck.Discard(card);
    }

    internal bool Advance()
    {
        _round++;
        _playerTurnOrder.MoveNext();

        var player = _playerTurnOrder.Current;
        _logger.LogDebug("State: {}", this);

        var playerChoosenCard = player.ChooseCardToPlay(CanBePlayed);
        if (playerChoosenCard == null)
        {
            _logger.LogDebug("{} did not play a card and has to draw", player);
            GiveCardTo(player);
            return true;
        }

        if (!CanBePlayed(LastPlayedCard))
            throw new IllegalMoveException($"{player} tried to play {playerChoosenCard} onto {LastPlayedCard}!");

        _logger.LogInformation("{Player} plays {Card}", player, playerChoosenCard);
        PlayCard(playerChoosenCard);

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

        return true;
    }

    public void GiveCardTo(IWriteOnlyPlayer playerToDraw)
    {
        var drawCard = TakeFromDrawStack();
        playerToDraw.PickupCard(drawCard);
    }

    public void Reverse() => _playerTurnOrder.Reverse();

    public void SkipTurns(int turnsToSkip)
    {
        for (int i = 0; i < turnsToSkip; i++)
            _playerTurnOrder.MoveNext();
    }

    public void AddPlayabilityFilterForNextTurn(IPlayabilityFilter filter)
    {
        _playabilityFiltersForNextTurn.Add(filter);
    }

    public bool CanBePlayed(ICard card)
    {
        foreach (var filter in _playabilityFiltersForNextTurn)
        {
            if (!filter.IsPlayble(card))
            {
                _logger.LogTrace("{} cannot be played onto {} because of filter {}", card, LastPlayedCard, filter);

            }
        }

        var result = card.CardType switch
        {
            _ when card.CardType == LastPlayedCard.CardType => true,
            _ when LastPlayedCard.CardType is not IColoredCardType && LastPlayedCard.CardType is not INumberedCardType => true,
            IColoredCardType cardColored when LastPlayedCard.CardType is IColoredCardType lastColored && cardColored.Color == lastColored.Color => true,
            INumberedCardType cardNumbered when LastPlayedCard.CardType is INumberedCardType lastNumbered && cardNumbered.Number == lastNumbered.Number => true,
            IColoredCardType => false,
            INumberedCardType => false,
            _ => true
        };
        if (!result)
            _logger.LogTrace("{} cannot be played onto {}", card, LastPlayedCard);
        return result;
    }

    IWriteOnlyPlayer IWriteOnlyGame.ToWriteOnly(IReadOnlyPlayer player) => (Player)player;

    public override string ToString() => $"[Game Round={_round} CurrentPlayer={_playerTurnOrder.Current} LastPlayed={_lastPlayedCard}]";

}
