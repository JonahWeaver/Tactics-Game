
public enum StatModType
{
    Flat,
    Percent
}
public class StatModifier
{
    public readonly int Value;
    public readonly StatModType Type;
    public readonly object Source;

    public StatModifier(int value, object source)
    {
        Value = value;
        Source = source;
    }
}
