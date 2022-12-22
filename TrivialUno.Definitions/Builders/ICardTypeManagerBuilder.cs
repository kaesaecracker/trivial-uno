namespace TrivialUno.Definitions.Builders;

public interface ICardTypeManagerBuilder
{
    public ICardTypeManagerBuilder AddCardType<T>();
    public ICardTypeManagerBuilder AddEffect<T>();
}
