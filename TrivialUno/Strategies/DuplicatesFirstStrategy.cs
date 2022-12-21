namespace TrivialUno.Strategies;

sealed class DuplicatesFirstStrategy : Strategy
{
    public DuplicatesFirstStrategy(
        ILogger<Strategy> logger,
        NextTurnParts.DuplicatesCardTypes duplicatesPart, NextTurnParts.RandomChoice randomPart, NextTurnParts.PopularColor popularColorPart)
        : base(logger)
    {
        NextTurn.Add(duplicatesPart);
        NextTurn.Add(popularColorPart);
        NextTurn.Add(randomPart);
    }
}
