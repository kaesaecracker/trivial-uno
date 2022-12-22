namespace TrivialUno.DefaultEffects;
public class SkipPlayerEffect : ICardEffect
{
    public Action<IWriteOnlyGame> Apply(IReadOnlyGame game) => write => write.SkipTurns(1);
}
