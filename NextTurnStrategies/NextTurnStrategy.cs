using Microsoft.Extensions.Logging;

namespace TrivialUno.NextTurnStrategies;

public interface INextTurnStrategylet
{
    public IReadOnlyList<Card> FilterOptions(IReadOnlyList<Card> hand, IReadOnlyList<Card> remainingOptions, Card currentTopCard);
}

public class NextTurnStrategy
{
    private readonly ILogger _logger = LoggingManager.Factory.CreateLogger<NextTurnStrategy>();

    public required INextTurnStrategylet[] Strategylets { private get; init; }

    public Card? GetNextTurn(IReadOnlyList<Card> hand, Card currentTopCard)
    {
        var remainingOptions = hand;
        foreach (var strategylet in Strategylets)
        {
            remainingOptions = strategylet.FilterOptions(hand, remainingOptions, currentTopCard);
            if (remainingOptions.Count == 0)
                return null;
            if (remainingOptions.Count == 1)
                return remainingOptions[0];
        }

        _logger.LogWarning("Cannot decide between the following cards: {}", remainingOptions);
        return remainingOptions[0];
    }

    public override string ToString() => $"[Strategy {Strategylets.Select(s => s.GetType().Name).Aggregate((a, b) => $"{a}, {b}")}]";
}
