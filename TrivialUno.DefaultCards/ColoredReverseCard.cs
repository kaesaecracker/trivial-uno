namespace TrivialUno.DefaultCards;

[DuplicatesPerDeck(2)]
[OneVariantPerColor]
[HasEffect(typeof(ReverseEffect))]
public sealed class ColoredReverseCard : IColoredCardType, IEffectCardType
{
    public required CardColor Color { get; set; }

    public string Name => $"{Color} reverse";

    public required IReadOnlyList<ICardEffect> Effects { get; set; }
}
