namespace TrivialUno.Definitions.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class HasEffectAttribute : UnoAttribute
{
    public HasEffectAttribute(Type effect)
    {
        if (!effect.IsAssignableTo(typeof(ICardEffect)))
            throw new ArgumentException($"Type has to implement {nameof(ICardEffect)}", nameof(effect));
        EffectType = effect;
    }

    public Type EffectType { get; }
}
