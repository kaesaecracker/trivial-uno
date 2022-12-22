using TrivialUno.Definitions;

namespace TrivialUno.DefaultEffects;

public abstract class ForceNextPlayerDrawEffect : ICardEffect
{
    internal ForceNextPlayerDrawEffect() { }

    public abstract int CardsToDraw { get; }

    public Action<IWriteOnlyGame> Apply(IReadOnlyGame game)
    {
        ArgumentNullException.ThrowIfNull(game);
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
    public override int CardsToDraw => 2;
}

public class ForceNextPlayerDraw4Effect : ForceNextPlayerDrawEffect
{
    public override int CardsToDraw => 4;
}
