using Microsoft.Extensions.Hosting;

namespace TrivialUno;

class TestGameHostedService : IHostedService
{
    private readonly ILogger<TestGameHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHost _host;

    public TestGameHostedService(ILogger<TestGameHostedService> logger, IServiceProvider serviceProvider, IHost host)
    {
        _logger = logger;
        _serviceProvider = serviceProvider.CreateScope().ServiceProvider;
        _host = host;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogTrace("TestGameHostedService.StartAsync");

        var players = _serviceProvider.GetRequiredService<Players>();
        var game = _serviceProvider.GetRequiredService<Game>();

        var p1 = _serviceProvider.GetRequiredService<Player>();
        p1.Name = "P1";
        p1.NextTurnStrategy = _serviceProvider.GetRequiredService<Strategies.FiFoStrategy>();
        players.Add(p1);

        var p2 = _serviceProvider.GetRequiredService<Player>();
        p2.Name = "P2";
        p2.NextTurnStrategy = _serviceProvider.GetRequiredService<Strategies.DuplicatesFirstStrategy>();
        players.Add(p2);

        game.SetupPhase();
        await game.Run();

        await _host.StopAsync(CancellationToken.None);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
