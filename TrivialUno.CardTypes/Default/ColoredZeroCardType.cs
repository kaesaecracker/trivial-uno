using TrivialUno.Definitions;
using TrivialUno.Definitions.Attributes;

namespace TrivialUno.CardTypes.Default;

[OneVariantPerColor]
public sealed class ColoredZeroCardType : IColoredCardType
{
    public required CardColor Color { get; set; }

    public string Name => $"{Color} 0";
}
