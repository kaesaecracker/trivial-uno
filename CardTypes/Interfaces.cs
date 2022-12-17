using TrivialUno.CardEffects;

namespace TrivialUno.CardTypes;

public interface ICardType
{
    uint CardsInDeck { get; }
}

public enum CardColor { Yellow, Red, Blue, Green }

public interface IColoredCardType : ICardType
{
    CardColor Color { get; }
}

public interface INumberedCardType : ICardType
{
    uint Number { get; }
}

public interface IEffectCardType : ICardType
{
    public IReadOnlyList<ICardEffect> CardEffects { get; }
}
