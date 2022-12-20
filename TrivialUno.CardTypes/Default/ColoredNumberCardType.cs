using TrivialUno.Definitions;
using TrivialUno.Definitions.Annotations;

namespace TrivialUno.CardTypes.Default;

[DuplicatesPerDeck(2)]
[OneVariantPerColor]
[OneVariantPerNumber]
public sealed class ColoredNumberCardType : INumberedCardType, IColoredCardType
{
    public required int Number { get; set; }

    public required CardColor Color { get; set; }
}
