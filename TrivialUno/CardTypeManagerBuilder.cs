using System.Reflection;
using TrivialUno.Definitions;
using TrivialUno.Definitions.Attributes;

namespace TrivialUno;

internal sealed class CardTypeManagerBuilder : ICardTypeManagerBuilder
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

        _logger.LogDebug("Adding card meta type {}", metaType.Name);
        var isNumbered = metaType.GetCustomAttribute<OneVariantPerNumberAttribute>() != null;
        var isColored = metaType.GetCustomAttribute<OneVariantPerColorAttribute>() != null;
        var cards = metaType.GetCustomAttribute<DuplicatesPerDeckAttribute>()?.CardsPerDeck ?? 1;
        var hasEffect = metaType.GetCustomAttributes<HasEffectAttribute>().Any();
        //if (hasEffect)
        //    throw new NotImplementedException();

        var value = new CardTypeGenerationData(cards, isColored, isNumbered);
        _logger.LogTrace("card meta type {} has the following properties: {}", metaType.Name, value);
        _generationData.Add(metaType, value);
        return this;
    }

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

    private static Func<ICardType> DefaultConstructor(Type t) => () => (ICardType)Activator.CreateInstance(t)!;

    private static IEnumerable<GenDataConstructor> GenerateColoredConstructors(GenDataConstructor gdc) => OptionalConstructor(gdc,
        check: genData => genData.IsColored,
        variantValues: Enum.GetValues<CardColor>(),
        variantApply: (instance, color) =>
        {
            if (instance is not IColoredCardType coloredInstance)
                throw new InvalidOperationException("colored card is not colored");
            coloredInstance.Color = color;
        });

    private static IEnumerable<GenDataConstructor> GenerateNumberedConstructors(GenDataConstructor gdc) => OptionalConstructor(gdc,
        check: genData => genData.IsNumbered,
        variantValues: Enumerable.Range(1, 7),
        variantApply: (instance, number) =>
        {
            if (instance is not INumberedCardType numbered)
                throw new InvalidOperationException("numbered card is not numbered");
            numbered.Number = number;
        });

    private static IEnumerable<GenDataConstructor> OptionalConstructor<T>(GenDataConstructor gdc, Func<CardTypeGenerationData, bool> check, IEnumerable<T> variantValues, Action<ICardType, T> variantApply)
    {
        if (!check(gdc.GenerationData))
        {
            yield return gdc;
            yield break;
        }

        foreach (var value in variantValues)
        {
            yield return gdc with
            {
                Constructor = () =>
                {
                    var instance = gdc.Constructor();
                    variantApply(instance, value);
                    return instance;
                }
            };
        }
    }

}
