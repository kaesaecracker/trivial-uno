using TrivialUno.Definitions;

namespace TrivialUno.Machinery;

sealed class Deck : IDeck
{
    private readonly ILogger<Deck> _logger;
    private readonly List<ICard> _discardPile;
    private readonly Random _random;
    private readonly Stack<ICard> _drawStack = new();

    public int CardsRemaining => _drawStack.Count;

    public Deck(ILogger<Deck> logger, IEnumerable<ICard> Cards, Random random)
    {
        _logger = logger;
        _discardPile = Cards.ToList();
        _random = new Random(random.Next());
    }

    public void Shuffle()
    {
        _logger.LogInformation("Shuffling discard pile into draw stack");
        while (_discardPile.Count > 0)
        {
            var card = _discardPile[_random.Next(_discardPile.Count)];
            _discardPile.Remove(card);
            _drawStack.Push(card);
        }
    }

    public ICard Draw()
    {
        if (!_drawStack.TryPop(out var card))
            throw new NotEnoughCardsException("did not have enough cards while drawing a card");
        _logger.LogDebug("took {} from draw stack", card);
        return card;
    }

    public void Discard(ICard card)
    {
        _logger.LogInformation("Discarding {}", card);
        _discardPile.Add(card);
    }
}
