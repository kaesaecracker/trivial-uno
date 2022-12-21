namespace TrivialUno.Definitions.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class DuplicatesPerDeckAttribute : UnoAttribute
{
    public DuplicatesPerDeckAttribute(int cardsPerDeck)
    {
        CardsPerDeck = cardsPerDeck;
    }

    public int CardsPerDeck { get; }
}
