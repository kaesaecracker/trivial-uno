namespace TrivialUno.CardEffects;

class ForceNextPlayerDrawEffect : ICardEffect
{
    public required uint CardsToDraw { get; init; }
}