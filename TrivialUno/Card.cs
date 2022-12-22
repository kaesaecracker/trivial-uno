using TrivialUno.Definitions;

namespace TrivialUno;

sealed class Card : ICard
{
    public required ICardType CardType { get; init; }

    public IEnumerable<ICardEffect> GetEffects() => CardType is IEffectCardType eff ? eff.Effects : Enumerable.Empty<ICardEffect>();

    public override string ToString() => $"[Card {CardType.Name}]";
}
