using TrivialUno.Definitions;

namespace TrivialUno.CardEffects;

public sealed class ReverseEffect : ICardEffect
{
    public void Apply(IGame game) => game.PlayerTurnOrder.Reverse();
}
