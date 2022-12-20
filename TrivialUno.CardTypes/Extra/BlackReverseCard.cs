using TrivialUno.CardEffects;
using TrivialUno.Definitions;
using TrivialUno.Definitions.Annotations;

namespace TrivialUno.CardTypes;

[DuplicatesPerDeck(2)]
[HasEffect(typeof(ReverseEffect))]
public sealed class BlackReverseCard : ICardType
{
}
