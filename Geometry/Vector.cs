namespace CityBuilder.Geometry;

public readonly struct Vector2 : IEquatable<Vector2>
{
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
    public readonly float X;
    public readonly float Y;
    public static Vector2 Zero { get { return new Vector2(0, 0); } }
    public static Vector2 One { get { return new Vector2(1, 1); } }
    public static Vector2 UnitX { get { return new Vector2(1, 0); } }
    public static Vector2 UnitY { get { return new Vector2(0, 1); } }
    public static float Dot(Vector2 a, Vector2 b) => a.X * b.X + a.Y * b.Y;
    public static float DistanceSquared(Vector2 a, Vector2 b)
    {
        Vector2 delta = a - b;
        return Dot(delta, delta);
    }
    public static bool operator ==(Vector2 a, Vector2 b)
    {
        return a.Equals((object)b);
    }
    public static bool operator !=(Vector2 a, Vector2 b)
    {
        return !(a == b);
    }
    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X + b.X, a.Y + b.Y);
    }
    public static Vector2 operator -(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X - b.X, a.Y - b.Y);
    }
    public static Vector2 operator -(Vector2 a)
    {
        return new Vector2(-a.X, -a.Y);
    }
    public static Vector2 operator *(Vector2 a, float b)
    {
        return new Vector2(a.X * b, a.Y * b);
    }
    public static Vector2 operator *(float a, Vector2 b) => b * a;
    public static Vector2 operator /(Vector2 a, float b)
    {
        if (b == 0) throw new Exception("Cannot divide Vector2 by 0");
        return new Vector2(a.X / b, a.Y / b);
    }
    public bool Equals(Vector2 other)
    {
        return EqualityComparer<float>.Default.Equals(X, other.X) && EqualityComparer<float>.Default.Equals(Y, other.Y);
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