using System.Reflection;
using TrivialUno.Definitions.Attributes;
using TrivialUno.Definitions.Builders;

namespace TrivialUno.Machinery;

internal sealed class CardTypeManagerBuilder : ICardTypeManagerBuilder
{
    private record struct CardTypeGenerationData(int CardsPerVariant, bool IsColored, bool IsNumbered, IReadOnlyList<Type> EffectTypes);
    private record struct CardGenDataConstructor(CardTypeGenerationData GenerationData, Func<ICardType> Constructor);

    private readonly Dictionary<Type, CardTypeGenerationData> _cardTypeGenerationData = new();
    private readonly IServiceProvider _services;
    private readonly ILogger<CardTypeManagerBuilder> _logger;

    internal CardTypeManagerBuilder(IServiceProvider services)
    {
        _services = services;
        _logger = services.GetRequiredService<ILogger<CardTypeManagerBuilder>>();
    }

    public ICardTypeManagerBuilder AddEffect<T>()
    {
        throw new NotImplementedException();
    }

    public ICardTypeManagerBuilder AddCardType<T>()
    {
        var metaType = typeof(T);
        if (_cardTypeGenerationData.ContainsKey(metaType))
            throw new InvalidOperationException($"Card type {metaType.Name} has already been added");
        if (!metaType.IsAssignableTo(typeof(ICardType)))
            throw new ArgumentException($"Type {metaType.Name} does not implement ICardType", nameof(T));

        _logger.LogDebug("Adding card meta type {}", metaType.Name);
        var isNumbered = metaType.GetCustomAttribute<OneVariantPerNumberAttribute>() != null;
        var isColored = metaType.GetCustomAttribute<OneVariantPerColorAttribute>() != null;
        var cards = metaType.GetCustomAttribute<DuplicatesPerDeckAttribute>()?.CardsPerDeck ?? 1;
        var effectTypes = metaType.GetCustomAttributes<HasEffectAttribute>().Select(attr => attr.EffectType).ToList().AsReadOnly();

        var value = new CardTypeGenerationData(cards, isColored, isNumbered, effectTypes);
        _logger.LogTrace("card meta type {} has the following properties: {}", metaType.Name, value);
        _cardTypeGenerationData.Add(metaType, value);
        return this;
    }

    internal ICardTypeManager Build()
    {
        var cardTypeInfos = _cardTypeGenerationData
            .Select(pair => new CardGenDataConstructor(pair.Value, DefaultConstructor(pair.Key)))
            .SelectMany(GenerateEffectConstructors)
            .SelectMany(GenerateColoredConstructors)
            .SelectMany(GenerateNumberedConstructors)
            // actually construct instances and transform to structure needed by CardTypeManager
            .Select(gdc => new CardTypeInfo(gdc.Constructor(), gdc.GenerationData.CardsPerVariant))
            .ToList();
        var deckConstructor = (IEnumerable<ICard> cards) => ActivatorUtilities.CreateInstance<Deck>(_services, cards);

        return ActivatorUtilities.CreateInstance<CardTypeManager>(
            _services, cardTypeInfos, deckConstructor);
    }

    private Func<ICardType> DefaultConstructor(Type t) => () => (ICardType)ActivatorUtilities.CreateInstance(_services, t);

    private static IEnumerable<CardGenDataConstructor> GenerateColoredConstructors(CardGenDataConstructor gdc) => OptionalVariantConstructor(gdc,
        check: genData => genData.IsColored,
        variantValues: Enum.GetValues<CardColor>(),
        variantApply: (genData, instance, color) =>
        {
            if (instance is not IColoredCardType coloredInstance)
                throw new ArgumentException("colored card is not colored", nameof(instance));
            coloredInstance.Color = color;
        });

    private static IEnumerable<CardGenDataConstructor> GenerateNumberedConstructors(CardGenDataConstructor gdc) => OptionalVariantConstructor(gdc,
        check: genData => genData.IsNumbered,
        variantValues: Enumerable.Range(1, 7),
        variantApply: (genData, instance, number) =>
        {
            if (instance is not INumberedCardType numbered)
                throw new ArgumentException("numbered card is not numbered", nameof(instance));
            numbered.Number = number;
        });

    private IEnumerable<CardGenDataConstructor> GenerateEffectConstructors(CardGenDataConstructor gdc)
    {
        if (!gdc.GenerationData.EffectTypes.Any())
        {
            yield return gdc;
            yield break;
        }
        yield return gdc with
        {
            Constructor = () =>
            {
                var instance = gdc.Constructor();
                if (instance is not IEffectCardType effectCardType)
                    throw new InvalidOperationException($"effect card type {instance.GetType().Name} is not an IEffectCardType");
                effectCardType.Effects = gdc.GenerationData.EffectTypes
                    .Select(t => (ICardEffect)ActivatorUtilities.CreateInstance(_services, t)).ToList();
                return instance;
            }
        };
    }

    private static IEnumerable<CardGenDataConstructor> OptionalVariantConstructor<T>(
        CardGenDataConstructor gdc, Func<CardTypeGenerationData, bool> check,
        IEnumerable<T> variantValues,
        Action<CardTypeGenerationData, ICardType, T> variantApply)
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
                    variantApply(gdc.GenerationData, instance, value);
                    return instance;
                }
            };
        }
    }
}
