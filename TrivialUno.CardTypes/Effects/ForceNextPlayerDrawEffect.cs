using TrivialUno.Definitions;

namespace TrivialUno.CardEffects;

public abstract class ForceNextPlayerDrawEffect : ICardEffect
{
    internal ForceNextPlayerDrawEffect() { }

    public abstract uint CardsToDraw { get; }

    public void Apply(IGame game)
    {
        var playerToDraw = game.PlayerTurnOrder.Next;
        for (int i = 0; i < CardsToDraw; i++)
            game.GiveCardTo(playerToDraw);
        game.PlayerTurnOrder.Skip(1);
    }
}

public class ForceNextPlayerDraw2Effect : ForceNextPlayerDrawEffect
{
    public override uint CardsToDraw => 2;
}

public class ForceNextPlayerDraw4Effect : ForceNextPlayerDrawEffect
{
    public override uint CardsToDraw => 4;
}
