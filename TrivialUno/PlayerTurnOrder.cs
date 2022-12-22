using TrivialUno.Definitions;

namespace TrivialUno;

sealed class PlayerTurnOrder : IPlayerTurnOrder
{
    private readonly ILogger<PlayerTurnOrder> _logger;
    private readonly Players _players;

    public PlayerTurnOrder(ILogger<PlayerTurnOrder> logger, Players players)
    {
        _logger = logger;
        _players = players;
    }

    private int _currentIndex;
    private int _direction = 1;
    private int _skip;

    public IPlayer Current => _players.Count == 0 ? ThrowNoPlayers() : _players[_currentIndex];

    public IPlayer Next => _players.Count == 0 ? ThrowNoPlayers() : _players[GetNextIndex()];

    private static Player ThrowNoPlayers() => throw new InvalidOperationException("no players added");

    public void MoveNext()
    {
        var oldIndex = _currentIndex;
        for (int i = 0; i <= _skip; i++)
            _currentIndex = GetNextIndex();
        _skip = 0;
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

    public void Skip(int v) => _skip++;
}
