namespace TrivialUno.Definitions;

public interface IPlayerTurnOrder
{
    public IPlayer Current { get; }

    public IPlayer Next { get; }

    public void MoveNext();

    public void Reverse();
}
