namespace TrivialUno.CardTypes;

public class CardTypeManager
{
    public static CardTypeManager Singleton { get; } = new();

    public IReadOnlyList<ICardType> Types => _types.AsReadOnly();

    private CardTypeManager()
    {
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

    private static readonly Random _random = new();
    private List<ICardType> _types = new();
}
