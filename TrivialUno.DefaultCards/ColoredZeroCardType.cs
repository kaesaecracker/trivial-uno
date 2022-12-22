namespace TrivialUno.DefaultCards;

[OneVariantPerColor]
public sealed class ColoredZeroCardType : IColoredCardType
{
    public required CardColor Color { get; set; }

    public string Name => $"{Color} 0";
}
