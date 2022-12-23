using TrivialUno.DefaultCards.Effects;
using TrivialUno.Definitions.Attributes;

namespace MyUno.CustomCards;

[DuplicatesPerDeck(2)]
[HasEffect(typeof(ReverseEffect))]
public sealed class BlackReverseCard : IEffectCardType
{
    public string Name => "reverse";

    public required IReadOnlyList<ICardEffect> Effects { get; set; }
}
