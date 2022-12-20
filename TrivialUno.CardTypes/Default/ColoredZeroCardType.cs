using TrivialUno.Definitions;
using TrivialUno.Definitions.Annotations;

namespace TrivialUno.CardTypes.Default;

[OneVariantPerColor]
public sealed class ColoredZeroCardType : IColoredCardType
{
    public required CardColor Color { get; set; }
}
