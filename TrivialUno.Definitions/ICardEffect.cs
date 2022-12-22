namespace TrivialUno.Definitions;

public interface ICardEffect
{
    public Action<IWriteOnlyGame> Apply(IReadOnlyGame game);
}
