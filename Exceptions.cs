namespace TrivialUno;

public class GameplayException : Exception
{
    public GameplayException() : base() { }
    public GameplayException(string message) : base(message) { }
    public GameplayException(string message, Exception inner) : base(message, inner) { }
}

public sealed class NotEnoughCardsException : GameplayException
{
    public NotEnoughCardsException() : base() { }
    public NotEnoughCardsException(string message) : base(message) { }
    public NotEnoughCardsException(string message, Exception inner) : base(message, inner) { }
}

public sealed class IllegalMoveException : GameplayException
{
    public IllegalMoveException() : base() { }
    public IllegalMoveException(string message) : base(message) { }
    public IllegalMoveException(string message, Exception inner) : base(message, inner) { }
}
