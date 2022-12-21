namespace TrivialUno.Definitions;

public interface IGame
{
    public IPlayerTurnOrder PlayerTurnOrder { get; }

    void GiveCardTo(IPlayer playerToDraw);

    bool CanBePlayed(ICard card);
}
