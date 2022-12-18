namespace TrivialUno.CardEffects;

sealed class ForceNextPlayerDrawEffect : ICardEffect
{
    public required uint CardsToDraw { get; init; }
}
