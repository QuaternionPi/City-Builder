namespace CityBuilder;

public class CustomValueType<TCustom, TValue>
{
    public CustomValueType(TValue value)
    {
        if (value is null) throw new NullReferenceException("Value cannot be null");
        Value = value;
    }
    protected readonly TValue Value;
    public override string ToString()
    {
        return Value?.ToString() ?? "";
    }
    public static bool operator <(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
    {
        return Comparer<TValue>.Default.Compare(a.Value, b.Value) < 0;
    }
    public static bool operator >(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
    {
        return !(a < b);
    }
    public static bool operator <=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
    {
        return (a < b) || (a == b);
    }
    public static bool operator >=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
    {
        return (a > b) || (a == b);
    }
    public static bool operator ==(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
    {
        return a.Equals((object)b);
    }
    public static bool operator !=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
    {
        return !(a == b);
    }
    public static TCustom operator +(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
    {
        return (dynamic)a.Value! + b.Value;
    }
    public static TCustom operator -(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
    {
        return (dynamic)a.Value! - b.Value;
    }
    protected bool Equals(CustomValueType<TCustom, TValue> other)
    {
        return EqualityComparer<TValue>.Default.Equals(Value, other.Value);
    }
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((CustomValueType<TCustom, TValue>)obj);
    }
    public override int GetHashCode()
    {
        return EqualityComparer<TValue>.Default.GetHashCode(Value!);
    }
}