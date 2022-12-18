using System.Globalization;
using System.Text;
using TrivialUno.CardEffects;
using TrivialUno.CardTypes;

namespace TrivialUno;

sealed class Card
{
    public required ICardType CardType { get; init; }

    public bool CanBePlayedOn(Card other) => other.CardType switch
    {
        IColoredCardType otherColored when CardType is IColoredCardType typeColorsed && otherColored.Color == typeColorsed.Color => true,
        INumberedCardType otherNumbered when CardType is INumberedCardType typeNumbered && otherNumbered.Number == typeNumbered.Number => true,
        _ => false
    };

    public override string ToString()
    {
        var builder = new StringBuilder("[Card");
        if (CardType is IColoredCardType typeColored)
            builder.Append(CultureInfo.InvariantCulture, $" {typeColored.Color}");
        if (CardType is INumberedCardType typeNumbered)
            builder.Append(CultureInfo.InvariantCulture, $" {typeNumbered.Number}");
        if (CardType is IEffectCardType typeEffect)
        {
            foreach (var effect in typeEffect.CardEffects)
            {
                if (effect is ForceNextPlayerDrawEffect drawEffect)
                    builder.Append(CultureInfo.InvariantCulture, $" draw{drawEffect.CardsToDraw}");
                if (effect is ChooseColorEffect)
                    builder.Append(" chooseColor");
            }
        }
        builder.Append(']');
        return builder.ToString();
    }
}
