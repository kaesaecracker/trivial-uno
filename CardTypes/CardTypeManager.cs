namespace TrivialUno.CardTypes;

sealed class CardTypeManager
{
    private readonly List<ICardType> _types = new();
    public IReadOnlyList<ICardType> Types => _types.AsReadOnly();
    private readonly Random _random;

    public CardTypeManager(Random rand)
    {
        _random = rand;

        foreach (CardColor color in Enum.GetValues(typeof(CardColor)))
        {
            _types.Add(new ColoredDrawCardType { Color = color });
            for (uint num = 1; num <= 7; num++)
            {
                _types.Add(new NormalCardType { Number = num, Color = color });
            }
        }

        _types.Add(new BlackDrawCardType());
        _types.Add(new BlackColorChooseCardType());
    }

    public Stack<Card> GenerateDrawStack()
    {
        var cards = new List<Card>();
        foreach (var t in Types)
        {
            for (int i = 0; i < t.CardsInDeck; i++)
            {
                cards.Add(new Card { CardType = t });
            }
        }

        var shuffled = cards.ToArray();
        _random.Shuffle(shuffled);

        var stack = new Stack<Card>(shuffled);
        return stack;
    }
}
