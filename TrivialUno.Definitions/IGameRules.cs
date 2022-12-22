namespace TrivialUno.Definitions;

public interface IGameRules
{
    int StartingCardsPerPlayer { get; }

    int MaxCardsInHand { get; }
}
