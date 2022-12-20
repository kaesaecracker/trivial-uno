using TrivialUno.CardEffects;
using TrivialUno.Definitions;
using TrivialUno.Definitions.Annotations;

namespace TrivialUno.CardTypes.Default;

[DuplicatesPerDeck(4)]
[HasEffect(typeof(ChooseColorEffect))]
[HasEffect(typeof(ForceNextPlayerDraw4Effect))]
public sealed class BlackDrawCardType : ICardType
{
}
