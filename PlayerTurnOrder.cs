namespace TrivialUno;

class PlayerTurnOrder : IEnumerator<Player>
{
    private readonly ILogger<PlayerTurnOrder> _logger;
    private readonly Players _players;

    public PlayerTurnOrder(ILogger<PlayerTurnOrder> logger, Players players)
    {
        _logger = logger;
        _players = players;
    }

    private int _currentIndex = 0;
    private int _direction = 1;

    public Player Current => _players[_currentIndex];

    object IEnumerator.Current => Current;

    void IDisposable.Dispose() { GC.SuppressFinalize(this); }

    public bool MoveNext()
    {
        var oldIndex = _currentIndex;
        _currentIndex = GetNextIndex();
        _logger.LogDebug("MoveNext moving from {} to {} with direction {}", oldIndex, _currentIndex, _direction);
        return true;
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void ReverseOrder()
    {
        _direction = -_direction;
    }

    public Player PeekNext() => _players[GetNextIndex()];

    private int GetNextIndex() => (_currentIndex + _direction) % _players.Count;
}

