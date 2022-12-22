namespace TrivialUno.Definitions;

public interface ICardTypeManagerBuilder
{
    public ICardTypeManagerBuilder AddCardType<T>();
    public ICardTypeManagerBuilder AddEffect<T>();
}
