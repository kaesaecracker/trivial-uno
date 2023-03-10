namespace TrivialUno.DefaultCards.Effects;
public class SkipPlayerEffect : ICardEffect
{
    public Action<IWriteOnlyGame> Apply(IReadOnlyGame game) => write => write.SkipTurns(1);
}
