using TrivialUno.Definitions.Attributes;

namespace MyUno.CustomCards;

// TODO: limit numbers for specific card type
[OneVariantPerNumber]
[HasEffect(typeof(EveryoneDrawsEffect))]
sealed class EveryoneDrawsCard : IEffectCardType, INumberedCardType
{
    public string Name => $"All+ {Number}";

    public required int Number { get; set; }

    public required IReadOnlyList<ICardEffect> Effects { get; set; }
}
