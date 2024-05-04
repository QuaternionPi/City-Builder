namespace CityBuilder.Numerics;

public readonly struct Vector2 : IEquatable<Vector2>
{
    public static implicit operator System.Numerics.Vector2(Vector2 v) => new(v.X, v.Y);
    public static implicit operator Vector2(System.Numerics.Vector2 v) => new(v.X, v.Y);
    public readonly float X;
    public readonly float Y;
    public Vector2(double x, double y)
    {
        X = (float)x;
        Y = (float)y;
    }
    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }
    public static Vector2 Zero { get { return new Vector2(0, 0); } }
    public static Vector2 One { get { return new Vector2(1, 1); } }
    public static Vector2 UnitX { get { return new Vector2(1, 0); } }
    public static Vector2 UnitY { get { return new Vector2(0, 1); } }
    public static float Dot(Vector2 a, Vector2 b) => a.X * b.X + a.Y * b.Y;
    public static float Distance(Vector2 a, Vector2 b) => MathF.Sqrt(DistanceSquared(a, b));
    public static float DistanceSquared(Vector2 a, Vector2 b)
    {
        Vector2 delta = a - b;
        return Dot(delta, delta);
    }
    public static Vector2 Lerp(Vector2 a, Vector2 b, float t) => a + (b - a) * t;
    public static Vector2 LerpClamped(Vector2 a, Vector2 b, float t) => Lerp(a, b, Math.Clamp(t, 0, 1));
    public static bool operator ==(Vector2 a, Vector2 b) => a.Equals((object)b);
    public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);
    public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
    public static Vector2 operator -(Vector2 a, Vector2 b) => a + (-b);
    public static Vector2 operator -(Vector2 a) => new Vector2(-a.X, -a.Y);
    public static Vector2 operator *(Vector2 a, float b) => new Vector2(a.X * b, a.Y * b);
    public static Vector2 operator *(float a, Vector2 b) => b * a;
    public static Vector2 operator /(Vector2 a, float b)
    {
        if (b == 0) throw new DivideByZeroException("Cannot divide Vector2 by 0");
        return new Vector2(a.X / b, a.Y / b);
    }
    public bool Equals(Vector2 other)
    {
        return EqualityComparer<float>.Default.Equals(X, other.X)
            && EqualityComparer<float>.Default.Equals(Y, other.Y);
    }
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Vector2)obj);
    }
    public override int GetHashCode()
    {
        var hashX = EqualityComparer<float>.Default.GetHashCode(X);
        return EqualityComparer<float>.Default.GetHashCode(Y + hashX);
    }
}