using TrivialUno.CardEffects;
using TrivialUno.Definitions;
using TrivialUno.Definitions.Attributes;

namespace TrivialUno.CardTypes.Default;

[DuplicatesPerDeck(2)]
[OneVariantPerColor]
[HasEffect(typeof(ForceNextPlayerDraw2Effect))]
public sealed class ColoredDrawCardType : IColoredCardType, IEffectCardType
{
    public required CardColor Color { get; set; }

    public string Name => $"{Color} +2";

    public required IReadOnlyList<ICardEffect> Effects { get; set; }
}
