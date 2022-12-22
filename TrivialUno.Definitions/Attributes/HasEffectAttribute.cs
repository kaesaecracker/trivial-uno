namespace TrivialUno.Definitions.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class HasEffectAttribute : UnoAttribute
{
    public HasEffectAttribute(Type effectType)
    {
        ArgumentNullException.ThrowIfNull(effectType);
        if (!effectType.IsAssignableTo(typeof(ICardEffect)))
            throw new ArgumentException($"Type has to implement {nameof(ICardEffect)}", nameof(effectType));
        EffectType = effectType;
    }

    public Type EffectType { get; }
}
