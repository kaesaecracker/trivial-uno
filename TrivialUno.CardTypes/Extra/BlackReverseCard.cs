using TrivialUno.CardEffects;
using TrivialUno.Definitions;
using TrivialUno.Definitions.Attributes;

namespace TrivialUno.CardTypes;

[DuplicatesPerDeck(2)]
[HasEffect(typeof(ReverseEffect))]
public sealed class BlackReverseCard : ICardType
{
    public string Name => "reverse";
}
