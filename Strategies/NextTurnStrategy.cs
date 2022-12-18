namespace TrivialUno.Strategies;

interface INextTurnStrategylet
{
    public IReadOnlyList<Card> FilterOptions(IReadOnlyList<Card> hand, IReadOnlyList<Card> remainingOptions, Card currentTopCard);
}

abstract class Strategy
{
    protected ILogger<Strategy> Logger { get; }
    protected List<INextTurnStrategylet> NextTurn { get; } = new();
    public Strategy(ILogger<Strategy> logger, NextTurnParts.Playable playablePart)
    {
        Logger = logger;
        NextTurn.Add(playablePart);
    }

    public Card? GetNextTurn(IReadOnlyList<Card> hand, Card currentTopCard)
    {
        var remainingOptions = hand;
        foreach (var strategylet in NextTurn)
        {
            remainingOptions = strategylet.FilterOptions(hand, remainingOptions, currentTopCard);
            if (remainingOptions.Count == 0)
                return null;
            if (remainingOptions.Count == 1)
                return remainingOptions[0];
        }

        Logger.LogWarning("Cannot decide between the following cards: {}", remainingOptions);
        return remainingOptions[0];
    }

    public override string ToString() => $"[Strategy {NextTurn.Select(s => s.GetType().Name).Aggregate((a, b) => $"{a}, {b}")}]";
}
