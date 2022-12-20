using TrivialUno.CardEffects;
using TrivialUno.Definitions;
using TrivialUno.Definitions.Annotations;

namespace TrivialUno.CardTypes.Default;

[DuplicatesPerDeck(2)]
[OneVariantPerColor]
[HasEffect(typeof(ForceNextPlayerDraw2Effect))]
public sealed class ColoredDrawCardType : IColoredCardType
{
    public required CardColor Color { get; set; }
}
