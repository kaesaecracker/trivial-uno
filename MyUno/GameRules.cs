namespace MyUno;

sealed class GameRules : IGameRules
{
    public int StartingCardsPerPlayer => 3;

    public int MaxCardsInHand => 7;
}
