namespace MyUno.ChoiceFilters;

sealed class RandomChoice : ICardChoiceFilter
{
    private readonly Random _random;
    public RandomChoice(Random rand)
    {
        _random = new Random(rand.Next());
    }

    public IReadOnlyList<ICard> FilterOptions(IReadOnlyList<ICard> hand, IReadOnlyList<ICard> remainingOptions)
        => new List<ICard>(1) { remainingOptions[_random.Next(remainingOptions.Count)] };
}
