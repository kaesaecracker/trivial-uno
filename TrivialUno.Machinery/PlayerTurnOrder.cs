namespace TrivialUno.Machinery;

sealed class PlayerTurnOrder
{
    private readonly ILogger<PlayerTurnOrder> _logger;
    private readonly IPlayers _players;

    public PlayerTurnOrder(ILogger<PlayerTurnOrder> logger, IPlayers players)
    {
        _logger = logger;
        _players = players;
    }

    private int _currentIndex;
    private int _direction = 1;

    public IPlayer Current => _players.Count == 0 ? ThrowNoPlayers() : _players[_currentIndex];

    public IPlayer Next => _players.Count == 0 ? ThrowNoPlayers() : _players[GetNextIndex()];

    private static Player ThrowNoPlayers() => throw new InvalidOperationException("no players added");

    public void MoveNext()
    {
        var oldIndex = _currentIndex;
        _currentIndex = GetNextIndex();
        _logger.LogDebug("MoveNext moving from {} to {} with direction {}", oldIndex, _currentIndex, _direction);
    }

    public void Reverse()
    {
        _direction = -_direction;
        _logger.LogInformation("player order reversed");
    }

    private int GetNextIndex()
    {
        var index = (_currentIndex + _direction) % _players.Count;
        return index < 0 ? index + _players.Count : index;
    }
}
