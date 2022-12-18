using Microsoft.Extensions.Hosting;

namespace TrivialUno;

sealed class TestGameHostedService : IHostedService, IDisposable
{
    private readonly ILogger<TestGameHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHost _host;
    private readonly IServiceScope _scope;
    private bool disposedValue;

    public TestGameHostedService(ILogger<TestGameHostedService> logger, IServiceProvider serviceProvider, IHost host)
    {
        _logger = logger;
        _host = host;
        _scope = serviceProvider.CreateScope();
        _serviceProvider = _scope.ServiceProvider;
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
        await game.Run().ConfigureAwait(false);

        await _host.StopAsync(CancellationToken.None).ConfigureAwait(false);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public void Dispose()
    {
        if (disposedValue)
            return;
        _scope.Dispose();
        disposedValue = true;
        GC.SuppressFinalize(this);
    }
}
