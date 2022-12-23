using TrivialUno.DefaultCards.Effects;

namespace TrivialUno.DefaultCards;

[DuplicatesPerDeck(2)]
[OneVariantPerColor]
[HasEffect(typeof(SkipPlayerEffect))]
public class ColoredSkipCardType : IColoredCardType, IEffectCardType
{
    public required CardColor Color { get; set; }

    public string Name => $"{Color} 🛇";

    public required IReadOnlyList<ICardEffect> Effects { get; set; }

}
