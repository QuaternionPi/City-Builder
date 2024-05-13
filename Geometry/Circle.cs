using CityBuilder.Numerics;

namespace CityBuilder.Geometry;

public readonly struct Circle
{
    public readonly Vector2 Center;
    public readonly float Radius;
    public Circle(Vector2 center, float radius)
    {
        Center = center;
        Radius = radius;
    }
    public static Circle operator +(Circle circle, Vector2 vector) =>
        new(circle.Center + vector, circle.Radius);
}

public readonly struct Sector
{
    public readonly Vector2 Center;
    public readonly Radian Start;
    public readonly Radian End;
    public readonly float Radius;
    public Sector(Vector2 center, Radian start, Radian end, float radius)
    {
        Center = center;
        Start = start;
        End = end;
        Radius = radius;
    }
    public static Sector operator +(Sector sector, Vector2 vector) =>
        new(sector.Center + vector, sector.Start, sector.End, sector.Radius);
}

public readonly struct Ring
{
    public readonly Vector2 Center;
    public readonly Radian Start;
    public readonly Radian End;
    public readonly float RadiusOuter;
    public readonly float RadiusInner;
    public Ring(Vector2 center, Radian start, Radian end, float radiusOuter, float radiusInner)
    {
        Center = center;
        Start = start;
        End = end;
        RadiusOuter = radiusOuter;
        RadiusInner = radiusInner;
    }
    public static Ring operator +(Ring ring, Vector2 vector) =>
        new(ring.Center + vector, ring.Start, ring.End, ring.RadiusOuter, ring.RadiusInner);
}
