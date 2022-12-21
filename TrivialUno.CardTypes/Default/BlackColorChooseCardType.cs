using TrivialUno.CardEffects;
using TrivialUno.Definitions;
using TrivialUno.Definitions.Attributes;

namespace TrivialUno.CardTypes.Default;

[DuplicatesPerDeck(4)]
[HasEffect(typeof(ChooseColorEffect))]
public sealed class BlackColorChooseCardType : ICardType
{
    public string Name => "ColorChooser";
}
