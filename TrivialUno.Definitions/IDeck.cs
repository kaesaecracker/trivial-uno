namespace TrivialUno.Definitions;

public interface IDeck
{
    void Shuffle(Random rand);

    void Discard(ICard card);

    ICard Draw();

    public int CardsRemaining { get; }
}
