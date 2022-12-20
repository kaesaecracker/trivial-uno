using TrivialUno.CardEffects;
using TrivialUno.Definitions;

namespace TrivialUno.CardTypes.Default;

public sealed class BlackDrawCardType : IEffectCardType, IBlackCardType
{
    public uint CardsInDeck => 4;

    public IReadOnlyList<ICardEffect> CardEffects { get; } = new List<ICardEffect>()
    {
        new ChooseColorEffect(),
        new ForceNextPlayerDrawEffect { CardsToDraw = 4 }
    };
}
