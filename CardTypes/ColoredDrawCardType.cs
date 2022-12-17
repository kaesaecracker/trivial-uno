using TrivialUno.CardEffects;

namespace TrivialUno.CardTypes;

class ColoredDrawCardType : IColoredCardType, IEffectCardType
{
    public required CardColor Color { get; init; }

    public uint CardsInDeck => 2;

    public IReadOnlyList<ICardEffect> CardEffects { get; } = new List<ICardEffect>()
    {
        new ForceNextPlayerDrawEffect { CardsToDraw = 2 }
    };
}
