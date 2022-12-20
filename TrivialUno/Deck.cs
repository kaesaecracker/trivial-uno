using System.Collections.Immutable;
using TrivialUno.Definitions;

namespace TrivialUno;

sealed class Deck
{
    private readonly List<ICard> _discardPile;
    private readonly Stack<ICard> _drawStack = new();

    public int CardsRemaining => _drawStack.Count;

    public Deck(IEnumerable<ICard> Cards)
    {
        _discardPile = Cards.ToList();
    }

    public void Shuffle(Random rand)
    {
        // _logger.LogInformation("Shuffling discard pile into draw stack");
        while (_discardPile.Count > 0)
        {
            var card = _discardPile[rand.Next(_discardPile.Count)];
            _discardPile.Remove(card);
            _drawStack.Push(card);
        }
    }

    public ICard Draw()
    {
        if (!_drawStack.TryPop(out var card))
            throw new NotEnoughCardsException("did not have enough cards while drawing a card");
        // _logger.LogDebug("took {} from draw stack", card);
        return card;
    }

    internal void Discard(ICard card)
    {
        // _logger.LogInformation("Discarding {}", card);
        _discardPile.Add(card);
    }
}
