using TrivialUno.DefaultCards.Effects;

namespace TrivialUno.DefaultCards;

[DuplicatesPerDeck(4)]
[HasEffect(typeof(ChooseColorEffect))]
[HasEffect(typeof(ForceNextPlayerDraw4Effect))]
public sealed class BlackDrawCardType : ICardType, IEffectCardType
{
    public string Name => "+4";
    public required IReadOnlyList<ICardEffect> Effects { get; set; }
}
