using TrivialUno.CardTypes;

namespace TrivialUno;

static class StaticStuff
{
    public static void Shuffle<T>(this Random rng, T[] array)
    {
        var n = array.Length;
        while (n > 1)
        {
            var k = rng.Next(n--);
            (array[k], array[n]) = (array[n], array[k]);
        }
    }

    public static string ToPrettyString(this IEnumerable<ICardType> cards)
    {
        return cards.Select(c => c.ToString()).Aggregate((a, b) => $"{a}, {b}") ?? "";
    }
}