using TrivialUno.CardTypes.Default;
using TrivialUno.Definitions;

namespace TrivialUno.CardTypes;

sealed class CardTypeManager
{
    private readonly List<ICardType> _types = new();
    public IReadOnlyList<ICardType> Types => _types.AsReadOnly();
    private readonly Random _random;

    public CardTypeManager(Random rand, GameRules rules)
    {
        _random = rand;

        foreach (CardColor color in Enum.GetValues(typeof(CardColor)))
        {
            _types.Add(new ColoredDrawCardType { Color = color });
            _types.Add(new ColoredReverseCard { Color = color });
            for (uint num = 1; num <= 7; num++)
            {
                _types.Add(new NormalCardType { Number = num, Color = color });
            }
        }

        _types.Add(new BlackDrawCardType());
        _types.Add(new BlackColorChooseCardType());

        if (rules.CrazyCards)
        {
            _types.Add(new BlackReverseCard());
        }
    }

    public Stack<ICard> GenerateDrawStack()
    {
        var cards = new List<ICard>();
        foreach (var t in Types)
        {
            for (int i = 0; i < t.CardsInDeck; i++)
            {
                cards.Add(new Card { CardType = t });
            }
        }

        var shuffled = cards.ToArray();
        _random.Shuffle(shuffled);

        var stack = new Stack<ICard>(shuffled);
        return stack;
    }
}
