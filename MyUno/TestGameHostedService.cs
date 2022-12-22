using System.Linq.Expressions;
using Microsoft.Extensions.Hosting;

namespace MyUno;

// TODO: this currently has to use many internal classes
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

        var players = _serviceProvider.GetRequiredService<IPlayers>();
        var game = _serviceProvider.GetRequiredService<IGame>();
        var stratManager = _serviceProvider.GetRequiredService<IStrategyManager>();

        players.Add("P1", stratManager.GetStrategy("FiFo"));
        players.Add("P1", stratManager.GetStrategy("DuplicatesFirst"));

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
