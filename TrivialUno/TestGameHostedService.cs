using Microsoft.Extensions.Hosting;

namespace TrivialUno;

sealed class TestGameHostedService : IHostedService, IDisposable
{
    private readonly ILogger<TestGameHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHost _host;
    private readonly IServiceScope _scope;
    private bool disposedValue;
    private Task? _backgroundTask;
    private readonly CancellationTokenSource _cts = new();

    public TestGameHostedService(ILogger<TestGameHostedService> logger, IServiceProvider serviceProvider, IHost host)
    {
        _logger = logger;
        _host = host;
        _scope = serviceProvider.CreateScope();
        _serviceProvider = _scope.ServiceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogTrace("TestGameHostedService.StartAsync");
        _backgroundTask = Task.Run(RunAsync, _cts.Token);
        return Task.CompletedTask;
    }

    private async Task? RunAsync()
    {
        _logger.LogTrace("TestGameHostedService.RunAsync");

        var players = _serviceProvider.GetRequiredService<Players>();
        var game = _serviceProvider.GetRequiredService<Game>();

        var p1 = _serviceProvider.GetRequiredService<Player>();
        p1.Name = "P1";
        p1.PlayCardStrategy = _serviceProvider.GetRequiredService<Strategies.FiFoStrategy>();
        players.Add(p1);

        var p2 = _serviceProvider.GetRequiredService<Player>();
        p2.Name = "P2";
        p2.PlayCardStrategy = _serviceProvider.GetRequiredService<Strategies.DuplicatesFirstStrategy>();
        players.Add(p2);

        try
        {
            await game.Run(_cts.Token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Oops, something unexpected happened");
            throw;
        }
        finally
        {
            if (!_cts.IsCancellationRequested)
                _ = _host.StopAsync(CancellationToken.None);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _cts.Cancel();
        if (_backgroundTask != null)
            await _backgroundTask.ConfigureAwait(false);
    }

    public void Dispose()
    {
        if (disposedValue)
            return;
        _scope.Dispose();
        _cts.Dispose();
        disposedValue = true;
        GC.SuppressFinalize(this);
    }
}
