using Microsoft.Extensions.Logging;
using TrivialUno.NextTurnStrategies;

namespace TrivialUno;

class Program
{
    private static readonly ILogger _logger = LoggingManager.Factory.CreateLogger<Program>();

    public static void Main()
    {
        _logger.LogTrace("Main()");

        var g = new Game
        {
            Players = new Player[] {
                new() {
                    Name = "FiFo",
                    NextTurnStrategy = new NextTurnStrategy
                    {
                        Strategylets = new INextTurnStrategylet[] {
                            PlayableStrategylet.Singleton,
                            FiFoStrategylet.Singleton,
                        },
                    }
                },
                new() {
                    Name = "DuplicatesFirst",
                    NextTurnStrategy = new NextTurnStrategy
                    {
                        Strategylets = new INextTurnStrategylet[] {
                            PlayableStrategylet.Singleton,
                            DuplicatesCardTypesFirstStrategylet.Singleton,
                            FiFoStrategylet.Singleton,
                        },
                    }
                },
                new() {
                    Name = "Complex",
                    NextTurnStrategy = new NextTurnStrategy
                    {
                        Strategylets = new INextTurnStrategylet[] {
                            PlayableStrategylet.Singleton,
                            PopularColorStrategylet.Singleton,
                            DuplicatesCardTypesFirstStrategylet.Singleton,
                            FiFoStrategylet.Singleton,
                        },
                    }
                }
            },
        };

        g.SetupPhase();
        while (!g.Ended)
            g.Advance();
    }
}