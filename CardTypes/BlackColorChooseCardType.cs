using TrivialUno.CardEffects;

namespace TrivialUno.CardTypes;

sealed class BlackColorChooseCardType : IEffectCardType, IBlackCardType
{
    public uint CardsInDeck => 4;

    public IReadOnlyList<ICardEffect> CardEffects { get; } = new List<ICardEffect>()
    {
        new ChooseColorEffect()
    };
}