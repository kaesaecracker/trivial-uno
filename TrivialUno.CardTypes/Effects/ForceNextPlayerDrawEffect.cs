using TrivialUno.Definitions;

namespace TrivialUno.CardEffects;

public abstract class ForceNextPlayerDrawEffect : ICardEffect
{
    internal ForceNextPlayerDrawEffect() { }

    public abstract uint CardsToDraw { get; }

    public Action<IWriteOnlyGame> Apply(IReadOnlyGame game)
    {
        var playerToDraw = game.NextPlayer;
        return actions =>
        {
            var modifiablePlayer = actions.ToWriteOnly(playerToDraw);
            for (int i = 0; i < CardsToDraw; i++)
                actions.GiveCardTo(modifiablePlayer);
            actions.SkipTurns(1);
        };
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
