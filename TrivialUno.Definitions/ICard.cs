namespace TrivialUno.Definitions;

public interface ICard
{
    ICardType CardType { get; init; }

    public bool CanBePlayedOn(ICard other);
}
