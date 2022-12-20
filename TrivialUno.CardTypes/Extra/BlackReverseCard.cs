using TrivialUno.CardEffects;
using TrivialUno.Definitions;

namespace TrivialUno.CardTypes;

public sealed class BlackReverseCard : IBlackCardType, IEffectCardType
{
    public uint CardsInDeck => 2;

    public IReadOnlyList<ICardEffect> CardEffects { get; } = new List<ICardEffect>()
    {
        new ReverseEffect()
    };
}
