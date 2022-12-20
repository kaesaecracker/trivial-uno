namespace TrivialUno.Definitions.Annotations;

[AttributeUsage(AttributeTargets.Class)]
public sealed class DuplicatesPerDeckAttribute : Attribute
{
    public DuplicatesPerDeckAttribute(int cardsPerDeck)
    {
        CardsPerDeck = cardsPerDeck;
    }

    public int CardsPerDeck { get; }
}
