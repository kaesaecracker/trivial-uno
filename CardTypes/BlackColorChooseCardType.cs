using TrivialUno.CardEffects;

namespace TrivialUno.CardTypes;

public class BlackColorChooseCardType : IEffectCardType
{
    public uint CardsInDeck => 4;

    public IReadOnlyList<ICardEffect> CardEffects { get; } = new List<ICardEffect>()
    {
        new ChooseColorEffect()
    };
}
