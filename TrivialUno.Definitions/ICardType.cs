namespace TrivialUno.Definitions;

public interface ICardType
{
    uint CardsInDeck { get; }
}

public interface IColoredCardType : ICardType
{
    CardColor Color { get; }
}

public interface IBlackCardType : ICardType
{
}

public interface INumberedCardType : ICardType
{
    uint Number { get; }
}

public interface IEffectCardType : ICardType
{
    public IReadOnlyList<ICardEffect> CardEffects { get; }
}
