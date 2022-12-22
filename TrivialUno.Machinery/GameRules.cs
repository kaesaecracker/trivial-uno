namespace TrivialUno.Machinery;

sealed class GameRules
{
    public uint StartingCardsPerPlayer { get; } = 3;

    public uint MaxCardsInHand { get; } = 7;

    public bool CrazyCards { get; } = true;
}
