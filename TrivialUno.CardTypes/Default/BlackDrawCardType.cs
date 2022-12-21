using TrivialUno.CardEffects;
using TrivialUno.Definitions;
using TrivialUno.Definitions.Attributes;

namespace TrivialUno.CardTypes.Default;

[DuplicatesPerDeck(4)]
[HasEffect(typeof(ChooseColorEffect))]
[HasEffect(typeof(ForceNextPlayerDraw4Effect))]
public sealed class BlackDrawCardType : ICardType
{
    public string Name => "+4";
}
