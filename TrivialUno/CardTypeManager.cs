using System.Reflection;
using TrivialUno.Definitions;
using TrivialUno.Definitions.Attributes;

namespace TrivialUno;

static class CardTypeManagerExtensions
{
    public static IServiceCollection AddCardTypeManager(this IServiceCollection services, Action<ICardTypeManagerBuilder> buildAction) => services
        .AddSingleton(services =>
        {
            var ctmBuilder = new CardTypeManagerBuilder(services);
            buildAction(ctmBuilder);
            return ctmBuilder.Build();
        });
}


sealed class CardTypeManagerBuilder : ICardTypeManagerBuilder
{
    private record struct CardTypeGenerationData(int CardsPerVariant, bool IsColored, bool IsNumbered);
    private record struct GenDataConstructor(CardTypeGenerationData GenerationData, Func<ICardType> Constructor);

    private readonly Dictionary<Type, CardTypeGenerationData> _generationData = new();
    private readonly IServiceProvider _services;
    private readonly ILogger<CardTypeManagerBuilder> _logger;

    internal CardTypeManagerBuilder(IServiceProvider services)
    {
        _services = services;
        _logger = services.GetRequiredService<ILogger<CardTypeManagerBuilder>>();
    }

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

    private static Func<ICardType> DefaultConstructor(Type t) => () => (ICardType)Activator.CreateInstance(t)!;

    internal CardTypeManager Build()
    {
        var genDataConstructors = _generationData.Select(pair => new GenDataConstructor(pair.Value, DefaultConstructor(pair.Key)))
            .SelectMany(GenerateColoredConstructors)
            .SelectMany(GenerateNumberedConstructors);
        var cardTypeDuplicateCountDict = genDataConstructors
            .Select(gdc => new KeyValuePair<ICardType, int>(gdc.Constructor(), gdc.GenerationData.CardsPerVariant))
            .ToDictionary(gdc => gdc.Key, gdc => gdc.Value);
        return new(_services.GetRequiredService<ILogger<CardTypeManager>>())
        {
            Types = cardTypeDuplicateCountDict.Keys.ToList(),
            CardsPerType = cardTypeDuplicateCountDict
        };
    }

    private static IEnumerable<GenDataConstructor> GenerateColoredConstructors(GenDataConstructor gdc)
    {
        if (!gdc.GenerationData.IsColored)
        {
            yield return gdc;
            yield break;
        }

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
        if (!gdc.GenerationData.IsNumbered)
        {
            yield return gdc;
            yield break;
        }

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
    private readonly ILogger<CardTypeManager> _logger;

    internal required IReadOnlyList<ICardType> Types { get; init; }
    internal required Dictionary<ICardType, int> CardsPerType { get; init; }

    internal CardTypeManager(ILogger<CardTypeManager> logger)
    {
        _logger = logger;
    }

    private IEnumerable<ICard> GenerateCardsForType(ICardType cardType)
    {
        var maxCount = CardsPerType[cardType];
        _logger.LogDebug("Generating {} cards for type [{}]", maxCount, cardType.Name);
        for (int count = 0; count <= maxCount; count++)
            yield return new Card { CardType = cardType };
    }

    public Deck GenerateNewDeck() => new(Types.SelectMany(GenerateCardsForType));
}
