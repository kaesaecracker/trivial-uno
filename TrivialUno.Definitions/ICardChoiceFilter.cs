namespace TrivialUno.Definitions;

public interface ICardChoiceFilter
{
    public IReadOnlyList<ICard> FilterOptions(IReadOnlyList<ICard> hand, IReadOnlyList<ICard> remainingOptions);
}
