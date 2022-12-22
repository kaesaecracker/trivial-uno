namespace TrivialUno.DefaultEffects;

public sealed class ReverseEffect : ICardEffect
{
    public Action<IWriteOnlyGame> Apply(IReadOnlyGame game) => actions => actions.Reverse();
}
