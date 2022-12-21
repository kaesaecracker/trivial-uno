namespace TrivialUno.Strategies;

sealed class FiFoStrategy : Strategy
{
    public FiFoStrategy(ILogger<Strategy> logger, NextTurnParts.FirstDrawnFirstPlayed fifoPart) : base(logger)
    {
        NextTurn.Add(fifoPart);
    }
}
