using TrivialUno.Definitions.Builders;

[assembly: CLSCompliant(true)]

namespace TrivialUno.DefaultCards;

public static class DefaultCardsExtensions
{
    public static ICardTypeManagerBuilder AddDefaultCards(this ICardTypeManagerBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder
            .AddCardType<ColoredDrawCardType>()
            .AddCardType<ColoredReverseCard>()
            .AddCardType<ColoredZeroCardType>()
            .AddCardType<ColoredNumberCardType>()
            .AddCardType<BlackDrawCardType>()
            .AddCardType<BlackColorChooseCardType>()
            .AddCardType<ColoredSkipCardType>();
    }
}
