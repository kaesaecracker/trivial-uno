using TrivialUno.Definitions;

namespace TrivialUno;

sealed class Card : ICard
{
    public required ICardType CardType { get; init; }

    public bool CanBePlayedOn(ICard other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return other.CardType switch
        {
            IColoredCardType otherColored when CardType is IColoredCardType typeColorsed && otherColored.Color == typeColorsed.Color => true,
            INumberedCardType otherNumbered when CardType is INumberedCardType typeNumbered && otherNumbered.Number == typeNumbered.Number => true,
            _ => false
        };
    }

    public override string ToString() => $"[Card {CardType.Name}]";
}
