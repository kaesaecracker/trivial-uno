namespace TrivialUno.Strategies;

sealed class FiFoStrategy : Strategy
{
    public FiFoStrategy(ILogger<Strategy> logger, NextTurnParts.Playable playablePart, NextTurnParts.FirstDrawnFirstPlayed fifoPart) : base(logger, playablePart)
    {
        NextTurn.Add(fifoPart);
    }
}
