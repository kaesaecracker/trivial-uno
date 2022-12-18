using TrivialUno.CardEffects;

namespace TrivialUno.CardTypes;

interface ICardType
{
    uint CardsInDeck { get; }
}

enum CardColor { Yellow, Red, Blue, Green }

interface IColoredCardType : ICardType
{
    CardColor Color { get; }
}

interface IBlackCardType : ICardType
{
}

interface INumberedCardType : ICardType
{
    uint Number { get; }
}

interface IEffectCardType : ICardType
{
    public IReadOnlyList<ICardEffect> CardEffects { get; }
}
