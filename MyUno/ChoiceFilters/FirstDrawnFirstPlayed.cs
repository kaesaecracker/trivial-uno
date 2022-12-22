namespace MyUno.ChoiceFilters;

sealed class FirstDrawnFirstPlayed : ICardChoiceFilter
{
    public IReadOnlyList<ICard> FilterOptions(IReadOnlyList<ICard> hand, IReadOnlyList<ICard> remainingOptions)
        => new List<ICard>(1) { remainingOptions[0] };
}
