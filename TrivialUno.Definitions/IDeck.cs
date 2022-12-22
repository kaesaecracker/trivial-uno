namespace TrivialUno.Definitions;

public interface IDeck
{
    void Shuffle();

    void Discard(ICard card);

    ICard Draw();

    public int CardsRemaining { get; }
}
