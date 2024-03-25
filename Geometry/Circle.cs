using CityBuilder.Numerics;

namespace CityBuilder.Geometry;

public readonly struct Circle
{
    public Circle(Vector2 center, float radius)
    {
        Center = center;
        Radius = radius;
    }
    public readonly Vector2 Center;
    public readonly float Radius;
}

public readonly struct Sector
{
    public Sector(Vector2 center, Radian start, Radian end, float radius)
    {
        Center = center;
        Start = start;
        End = end;
        Radius = radius;
    }
    public readonly Vector2 Center;
    public readonly Radian Start;
    public readonly Radian End;
    public readonly float Radius;
}

public readonly struct Ring
{
    public Ring(Vector2 center, Radian start, Radian end, float radiusOuter, float radiusInner)
    {
        Center = center;
        Start = start;
        End = end;
        RadiusOuter = radiusOuter;
        RadiusInner = radiusInner;
    }
    public readonly Vector2 Center;
    public readonly Radian Start;
    public readonly Radian End;
    public readonly float RadiusOuter;
    public readonly float RadiusInner;
}
