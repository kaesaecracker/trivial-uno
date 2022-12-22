namespace TrivialUno.Definitions;

public interface IGame : IReadOnlyGame, IWriteOnlyGame { }

public interface IWriteOnlyGame
{
    void GiveCardTo(IWriteOnlyPlayer playerToDraw);

    void AddPlayabilityFilterForNextTurn(IPlayabilityFilter filter);

    void SkipTurns(int turnsToSkip);

    void Reverse();

    IWriteOnlyPlayer ToWriteOnly(IReadOnlyPlayer player);
}

public interface IReadOnlyGame
{
    IReadOnlyPlayer CurrentPlayer { get; }

    IReadOnlyPlayer NextPlayer { get; }

    bool CanBePlayed(ICard card);
}
