namespace CityBuilder.Numerics;

public readonly struct Degree(double value) : IEquatable<Degree>
{
    private readonly double Value = Normalize(value);
    public static implicit operator Degree(int value) { return new Degree(value); }
    public static implicit operator Degree(float value) { return new Degree(value); }
    public static implicit operator Degree(double value) { return new Degree(value); }
    public static implicit operator Degree(Radian value) { return new Degree(value * 180 / Math.PI); }
    public static implicit operator double(Degree Degree) { return Degree.Value; }
    private static double Normalize(double value)
    {
        return (value - 180) % (2 * 180) + 180;
    }
    public override string ToString()
    {
        return Value.ToString() ?? "";
    }
    public static bool operator <(Degree a, Degree b)
    {
        return Comparer<double>.Default.Compare(a.Value, b.Value) < 0;
    }
    public static bool operator >(Degree a, Degree b)
    {
        return !(a < b);
    }
    public static bool operator <=(Degree a, Degree b)
    {
        return (a < b) || (a == b);
    }
    public static bool operator >=(Degree a, Degree b)
    {
        return (a > b) || (a == b);
    }
    public static bool operator ==(Degree a, Degree b)
    {
        return a.Equals((object)b);
    }
    public static bool operator !=(Degree a, Degree b)
    {
        return !(a == b);
    }
    public static Degree operator +(Degree a, Degree b)
    {
        return a.Value! + b.Value;
    }
    public static Degree operator -(Degree a, Degree b)
    {
        return a.Value! - b.Value;
    }
    public bool Equals(Degree other)
    {
        return EqualityComparer<double>.Default.Equals(Value, other.Value);
    }
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Degree)obj);
    }
    public override int GetHashCode()
    {
        return EqualityComparer<double>.Default.GetHashCode(Value!);
    }
}