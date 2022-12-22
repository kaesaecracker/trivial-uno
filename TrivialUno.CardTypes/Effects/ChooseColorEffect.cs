using Microsoft.Extensions.Logging;
using TrivialUno.Definitions;

namespace TrivialUno.CardEffects;

public sealed class ChooseColorEffect : ICardEffect
{
    private readonly ILogger<ChooseColorEffect> _logger;

    public ChooseColorEffect(ILogger<ChooseColorEffect> logger)
    {
        _logger = logger;
    }

    public void Apply(IGame game)
    {
        _logger.LogCritical("ChooseColorEffect is not implemented");
    }
}
