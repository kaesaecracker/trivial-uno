using TrivialUno.Definitions;

namespace TrivialUno.DefaultEffects;

public sealed record class ChooseColorEffect(ILogger<ChooseColorEffect> Logger) : ICardEffect
{
    public Action<IWriteOnlyGame> Apply(IReadOnlyGame game)
    {
        ArgumentNullException.ThrowIfNull(game);
        var player = game.CurrentPlayer;
        return actions =>
        {
            var color = actions.ToWriteOnly(player).ChooseColor();
            Logger.LogInformation("The next card to be played has to be {Color}", color);
            var modifier = new ChosenColorFilter(color);
            actions.AddPlayabilityFilterForNextTurn(modifier);
        };
    }

    private sealed record class ChosenColorFilter(CardColor Color) : IPlayabilityFilter
    {
        public bool IsPlayble(ICard card) => card.CardType is IColoredCardType colored && colored.Color == Color;
    }
}
