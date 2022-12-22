using TrivialUno.Definitions;

namespace TrivialUno.CardEffects;

public sealed class ReverseEffect : ICardEffect
{
    public Action<IWriteOnlyGame> Apply(IReadOnlyGame _) => actions => actions.Reverse();
}
