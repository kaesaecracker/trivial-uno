namespace TrivialUno.Definitions;

public interface IReadOnlyPlayer
{
    int CardsLeft { get; }
}

public interface IWriteOnlyPlayer
{
    void PickupCard(ICard card);

    ICard? ChooseCardToPlay(Func<ICard, bool> canBePlayed);

    ICard ChooseCardToDiscard();

    CardColor ChooseColor();
}

public interface IPlayer : IReadOnlyPlayer, IWriteOnlyPlayer
{
}
