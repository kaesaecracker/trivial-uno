using TrivialUno.Definitions;

namespace TrivialUno.CardEffects;

public sealed class ForceNextPlayerDrawEffect : ICardEffect
{
    public required uint CardsToDraw { get; init; }
}
