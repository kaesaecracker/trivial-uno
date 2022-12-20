using TrivialUno.Definitions;

namespace TrivialUno.CardTypes.Default;

public sealed class NormalCardType : IColoredCardType, INumberedCardType
{
    public required uint Number { get; init; }

    public required CardColor Color { get; init; }

    public uint CardsInDeck => Number == 0u ? 1u : 2u;
}
