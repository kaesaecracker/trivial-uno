namespace TrivialUno.Machinery;

internal sealed class FilteredStrategy : IStrategy
{
    private readonly ILogger<FilteredStrategy> _logger;
    private readonly List<ICardChoiceFilter> _nextTurnFilters;

    public FilteredStrategy(ILogger<FilteredStrategy> logger, IEnumerable<ICardChoiceFilter> nextTurnFilters)
    {
        _logger = logger;
        _nextTurnFilters = nextTurnFilters.ToList();
    }

    public ICard? GetNextTurn(IReadOnlyList<ICard> hand, Func<ICard, bool> canBePlayed)
    {
        IReadOnlyList<ICard> remainingOptions = hand.Where(canBePlayed).ToList();
        // TODO how to de-dubplicate this?
        if (remainingOptions.Count == 0)
            return null;
        if (remainingOptions.Count == 1)
            return remainingOptions[0];

        foreach (var strategylet in _nextTurnFilters)
        {
            remainingOptions = strategylet.FilterOptions(hand, remainingOptions);
            if (remainingOptions.Count == 0)
                return null;
            if (remainingOptions.Count == 1)
                return remainingOptions[0];
        }

        _logger.LogWarning("Cannot decide between the following cards: {}", remainingOptions);
        return remainingOptions[0];
    }

    public override string ToString() => $"[Strategy {_nextTurnFilters.Select(s => s.GetType().Name).Aggregate((a, b) => $"{a}, {b}")}]";
}
