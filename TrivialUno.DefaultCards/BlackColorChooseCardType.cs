using TrivialUno.DefaultCards.Effects;

namespace TrivialUno.DefaultCards;

[DuplicatesPerDeck(4)]
[HasEffect(typeof(ChooseColorEffect))]
public sealed class BlackColorChooseCardType : ICardType, IEffectCardType
{
    public string Name => "ColorChooser";
    public required IReadOnlyList<ICardEffect> Effects { get; set; }
}
