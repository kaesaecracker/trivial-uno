using System.Threading.Tasks.Dataflow;

namespace TrivialUno.Definitions;

public interface IPlayer
{
    void PickupCard(ICard card);

    ICard? ChooseCardToPlay(Func<ICard, bool> canBePlayed);

    ICard ChooseCardToDiscard();

    int CardsLeft { get; }
}
