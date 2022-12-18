namespace TrivialUno.Strategies;

class DuplicatesFirstStrategy : Strategy
{
    public DuplicatesFirstStrategy(
        ILogger<Strategy> logger, NextTurnParts.Playable playablePart,
        NextTurnParts.DuplicatesCardTypes duplicatesPart, NextTurnParts.FirstDrawnFirstPlayed fifoPart, NextTurnParts.PopularColor popularColorPart)
        : base(logger, playablePart)
    {
        NextTurn.Add(duplicatesPart);
        NextTurn.Add(popularColorPart);
        NextTurn.Add(fifoPart);
    }
}