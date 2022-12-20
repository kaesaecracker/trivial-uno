using System.Reflection;
using TrivialUno.Definitions;
using TrivialUno.Definitions.Annotations;

namespace TrivialUno;

static class CardTypeManagerExtensions
{
    public static IServiceCollection AddCardTypeManager(this IServiceCollection services, Action<ICardTypeManagerBuilder> buildAction) => services
        .AddSingleton(_ =>
        {
            var ctmBuilder = new CardTypeManagerBuilder();
            buildAction(ctmBuilder);
            return ctmBuilder.Build();
        });
}


sealed class CardTypeManagerBuilder : ICardTypeManagerBuilder
{
    private record struct CardTypeGenerationData(int CardsPerVariant, bool IsColored, bool IsNumbered);
    private record struct GenDataConstructor(CardTypeGenerationData GenerationData, Func<ICardType> Constructor);

    private readonly Dictionary<Type, CardTypeGenerationData> _generationData = new();

    internal CardTypeManagerBuilder() { }

    public ICardTypeManagerBuilder AddCardType<T>()
    {
        var metaType = typeof(T);
        if (_generationData.ContainsKey(metaType))
            throw new InvalidOperationException($"Card type {metaType.Name} has already been added");
        if (!metaType.IsAssignableTo(typeof(ICardType)))
            throw new ArgumentException($"Type {metaType.Name} does not implement ICardType", nameof(T));

        var isNumbered = metaType.GetCustomAttribute<OneVariantPerNumberAttribute>() != null;
        var isColored = metaType.GetCustomAttribute<OneVariantPerColorAttribute>() != null;
        var cards = metaType.GetCustomAttribute<DuplicatesPerDeckAttribute>()?.CardsPerDeck ?? 1;
        var hasEffect = metaType.GetCustomAttributes<HasEffectAttribute>().Any();
        //if (hasEffect)
        //    throw new NotImplementedException();

        _generationData.Add(metaType, new CardTypeGenerationData(cards, isColored, isNumbered));
        return this;
    }

    private static Func<ICardType> DefaultConstructor(Type t)
        => () => (ICardType)Activator.CreateInstance(t)!;

    internal CardTypeManager Build()
    {
        var a = _generationData.Select(pair => new GenDataConstructor(pair.Value, DefaultConstructor(pair.Key)));
        var b = a.SelectMany(gdc => gdc.GenerationData.IsColored ? GenerateColoredConstructors(gdc) : new[] { gdc });
        var c = b.SelectMany(gdc => gdc.GenerationData.IsNumbered ? GenerateNumberedConstructors(gdc) : new[] { gdc });
        var d = c.Select(gdc => new KeyValuePair<ICardType, int>(gdc.Constructor(), gdc.GenerationData.CardsPerVariant));
        var types = d.ToDictionary(gdc => gdc.Key, gdc => gdc.Value);
        return new() { Types = types.Keys.ToList(), CardsPerType = types };
    }

    private static IEnumerable<GenDataConstructor> GenerateColoredConstructors(GenDataConstructor gdc)
    {
        foreach (CardColor color in Enum.GetValues(typeof(CardColor)))
        {
            var constuct = () =>
            {
                var instance = gdc.Constructor();
                if (instance is not IColoredCardType coloredInstance)
                    throw new InvalidOperationException("colored card is not colored");
                coloredInstance.Color = color;
                return instance;
            };
            yield return gdc with { Constructor = constuct };
        }
    }

    private static IEnumerable<GenDataConstructor> GenerateNumberedConstructors(GenDataConstructor gdc)
    {
        for (int number = 1; number <= 7; number++)
        {
            var constuct = () =>
            {
                var instance = gdc.Constructor();
                if (instance is not INumberedCardType numbered)
                    throw new InvalidOperationException("numbered card is not numbered");
                numbered.Number = number;
                return instance;
            };
            yield return gdc with { Constructor = constuct };
        }
    }
}

sealed class CardTypeManager
{
    internal required IReadOnlyList<ICardType> Types { get; init; }
    internal required Dictionary<ICardType, int> CardsPerType { get; init; }

    internal CardTypeManager() { }

    private IEnumerable<ICard> GenerateCardsForType(ICardType cardType)
    {
        var maxCount = CardsPerType[cardType];
        for (int count = 0; count <= maxCount; count++)
        {
            Console.WriteLine(cardType);
            yield return new Card { CardType = cardType };
        }
    }

    public Deck GenerateNewDeck() => new(Types.SelectMany(GenerateCardsForType));
}
