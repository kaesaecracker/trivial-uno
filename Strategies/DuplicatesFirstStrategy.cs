namespace TrivialUno.Strategies;

sealed class DuplicatesFirstStrategy : Strategy
{
    public DuplicatesFirstStrategy(
        ILogger<Strategy> logger, NextTurnParts.Playable playablePart,
        NextTurnParts.DuplicatesCardTypes duplicatesPart, NextTurnParts.RandomChoice randomPart, NextTurnParts.PopularColor popularColorPart)
        : base(logger, playablePart)
    {
        NextTurn.Add(duplicatesPart);
        NextTurn.Add(popularColorPart);
        NextTurn.Add(randomPart);
    }
}
