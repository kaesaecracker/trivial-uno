using TrivialUno.CardEffects;

namespace TrivialUno.CardTypes;

sealed class BlackDrawCardType : IEffectCardType
{
    public uint CardsInDeck => 4;

    public IReadOnlyList<ICardEffect> CardEffects { get; } = new List<ICardEffect>()
    {
        new ChooseColorEffect(),
        new ForceNextPlayerDrawEffect { CardsToDraw = 4 }
    };
}
