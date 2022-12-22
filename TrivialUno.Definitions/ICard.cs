namespace TrivialUno.Definitions;

public interface ICard
{
    ICardType CardType { get; }

    IEnumerable<ICardEffect> GetEffects();
}
