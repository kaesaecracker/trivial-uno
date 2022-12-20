using TrivialUno.Definitions;

namespace TrivialUno.CardEffects;

public interface IForceNextPlayerDrawEffect : ICardEffect
{
    public uint CardsToDraw { get; }
}

public class ForceNextPlayerDraw2Effect : IForceNextPlayerDrawEffect
{
    public uint CardsToDraw => 2;
}

public class ForceNextPlayerDraw4Effect : IForceNextPlayerDrawEffect
{
    public uint CardsToDraw => 4;
}
