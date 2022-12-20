using TrivialUno.CardEffects;
using TrivialUno.Definitions;
using TrivialUno.Definitions.Annotations;

namespace TrivialUno.CardTypes.Default;

[DuplicatesPerDeck(2)]
[OneVariantPerColor]
[HasEffect(typeof(ReverseEffect))]
public sealed class ColoredReverseCard : IColoredCardType
{
    public required CardColor Color { get; set; }
}
