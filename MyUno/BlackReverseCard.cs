using TrivialUno.DefaultEffects;
using TrivialUno.Definitions.Attributes;

namespace MyUno;

[DuplicatesPerDeck(2)]
[HasEffect(typeof(ReverseEffect))]
public sealed class BlackReverseCard : IEffectCardType
{
    public string Name => "reverse";

    public required IReadOnlyList<ICardEffect> Effects { get; set; }
}
