namespace TrivialUno.Definitions;

public interface ICardType
{
}

public interface IColoredCardType : ICardType
{
    CardColor Color { get; set; }
}

public interface INumberedCardType : ICardType
{
    int Number { get; set; }
}
