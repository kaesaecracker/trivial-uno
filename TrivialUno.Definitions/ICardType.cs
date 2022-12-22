namespace TrivialUno.Definitions;

public interface ICardType
{
    public string Name { get; }
}

public interface IColoredCardType : ICardType
{
    CardColor Color { get; set; }
}

public interface INumberedCardType : ICardType
{
    int Number { get; set; }
}

public interface IEffectCardType : ICardType
{
    IReadOnlyList<ICardEffect> Effects { get; set; }
}
