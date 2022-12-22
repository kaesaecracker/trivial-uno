namespace MyUno.CustomCards;

sealed class EveryoneDrawsEffect : ICardEffect
{
    private readonly ILogger<EveryoneDrawsEffect> _logger;

    public EveryoneDrawsEffect(ILogger<EveryoneDrawsEffect> logger)
    {
        _logger = logger;
    }

    public Action<IWriteOnlyGame> Apply(IReadOnlyGame game)
    {
        var playersToDraw = game.AllPlayers()
            .Where(p => p != game.CurrentPlayer)
            .ToList();
        return write =>
        {
            _logger.LogWarning("EVERYONE HAS TO DRAW 🤯");
            foreach (var player in playersToDraw)
                write.GiveCardTo(write.ToWriteOnly(player));
        };
    }
}
