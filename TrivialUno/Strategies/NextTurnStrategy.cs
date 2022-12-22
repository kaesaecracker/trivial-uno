using TrivialUno.Definitions;

namespace TrivialUno.Strategies;

interface INextTurnPart
{
    public IReadOnlyList<ICard> FilterOptions(IReadOnlyList<ICard> hand, IReadOnlyList<ICard> remainingOptions);
}

abstract class Strategy
{
    protected ILogger<Strategy> Logger { get; }
    protected List<INextTurnPart> NextTurn { get; } = new();
    public Strategy(ILogger<Strategy> logger)
    {
        Logger = logger;
    }

    public ICard? GetNextTurn(IReadOnlyList<ICard> hand, Func<ICard, bool> canBePlayed)
    {
        IReadOnlyList<ICard> remainingOptions = hand.Where(canBePlayed).ToList();
        // TODO how to de-dubplicate this?
        if (remainingOptions.Count == 0)
            return null;
        if (remainingOptions.Count == 1)
            return remainingOptions[0];

        foreach (var strategylet in NextTurn)
        {
            remainingOptions = strategylet.FilterOptions(hand, remainingOptions);
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
