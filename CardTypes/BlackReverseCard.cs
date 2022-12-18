using TrivialUno.CardEffects;

namespace TrivialUno.CardTypes;

sealed class BlackReverseCard : IBlackCardType, IEffectCardType
{
    public uint CardsInDeck => 2;

    public IReadOnlyList<ICardEffect> CardEffects { get; } = new List<ICardEffect>()
    {
        new ReverseEffect()
    };
}
