namespace CityBuilder.Geometry;

public readonly struct Radian(double value) : IEquatable<Radian>
{
    private readonly double Value = Normalize(value);
    public static implicit operator Radian(int value) { return new Radian(value); }
    public static implicit operator Radian(float value) { return new Radian(value); }
    public static implicit operator Radian(double value) { return new Radian(value); }
    public static implicit operator Radian(Degree value) { return new Radian(value * Math.PI / 180); }
    public static implicit operator double(Radian radian) { return radian.Value; }
    private static double Normalize(double value)
    {
        return (value - Math.PI) % (2 * Math.PI) + Math.PI;
    }
    public override string ToString()
    {
        return Value.ToString() ?? "";
    }
    public static bool operator <(Radian a, Radian b)
    {
        return Comparer<double>.Default.Compare(a.Value, b.Value) < 0;
    }
    public static bool operator >(Radian a, Radian b)
    {
        return !(a < b);
    }
    public static bool operator <=(Radian a, Radian b)
    {
        return (a < b) || (a == b);
    }
    public static bool operator >=(Radian a, Radian b)
    {
        return (a > b) || (a == b);
    }
    public static bool operator ==(Radian a, Radian b)
    {
        return a.Equals((object)b);
    }
    public static bool operator !=(Radian a, Radian b)
    {
        return !(a == b);
    }
    public static Radian operator +(Radian a, Radian b)
    {
        return a.Value! + b.Value;
    }
    public static Radian operator -(Radian a, Radian b)
    {
        return a.Value! - b.Value;
    }
    public bool Equals(Radian other)
    {
        return EqualityComparer<double>.Default.Equals(Value, other.Value);
    }
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Radian)obj);
    }
    public override int GetHashCode()
    {
        return EqualityComparer<double>.Default.GetHashCode(Value!);
    }
}