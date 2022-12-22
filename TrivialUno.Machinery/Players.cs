namespace TrivialUno.Machinery;

sealed class Players : IPlayers
{
    private readonly List<IPlayer> _allPlayers = new();
    private readonly IServiceProvider _serviceProvider;

    public Players(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Add(string name, IStrategy strat)
    {
        var player = ActivatorUtilities.CreateInstance<Player>(_serviceProvider, name, strat);
        _allPlayers.Add(player);
    }

    public IEnumerator<IPlayer> GetEnumerator() => _allPlayers.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IPlayer this[int key]
    {
        get => _allPlayers[key];
    }

    public int Count => _allPlayers.Count;
}
