using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TrivialUno.Strategies;

public interface INextTurnStrategylet
{
    public IReadOnlyList<Card> FilterOptions(IReadOnlyList<Card> hand, IReadOnlyList<Card> remainingOptions, Card currentTopCard);
}

public class NextTurnStrategy
{
    private readonly ILogger<NextTurnStrategy> _logger;
    public NextTurnStrategy(ILogger<NextTurnStrategy> logger)
    {
        _logger = logger;
    }

    public required INextTurnStrategylet[] Strategylets { private get; set; }

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

public class StrategyManager
{
    private Dictionary<string, NextTurnStrategy> _strategies;
    public StrategyManager(IServiceProvider provider)
    {
        var fifo = provider.GetRequiredService<NextTurnStrategy>();
        fifo.Strategylets = new INextTurnStrategylet[] {
            PlayableStrategylet.Singleton,
            FiFoStrategylet.Singleton,
        };

        var duplicates = provider.GetRequiredService<NextTurnStrategy>();
        duplicates.Strategylets = new INextTurnStrategylet[] {
            PlayableStrategylet.Singleton,
            DuplicatesCardTypesFirstStrategylet.Singleton,
            FiFoStrategylet.Singleton,
        };

        var complex = provider.GetRequiredService<NextTurnStrategy>();
        complex.Strategylets = new INextTurnStrategylet[] {
            PlayableStrategylet.Singleton,
            PopularColorStrategylet.Singleton,
            DuplicatesCardTypesFirstStrategylet.Singleton,
            FiFoStrategylet.Singleton,
        };

        _strategies = new()
        {
            { "FiFo", fifo },
            { "Duplicates", duplicates },
            { "Complex", complex },
        };
    }


    public NextTurnStrategy GetNextTurnStrategy(string name)
    {
        return _strategies[name];
    }
}
