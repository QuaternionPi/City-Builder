namespace CityBuilder.Geometry;

public interface IPolygon
{
    public Vector2[] Points { get; }
    public Vector2 Center { get; }
}

public readonly struct Line : IPolygon
{
    public Line(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;
    }
    public readonly Vector2 Start;
    public readonly Vector2 End;
    public readonly Vector2[] Points { get { return [Start, End]; } }
    public readonly Vector2 Center { get { return (Start + End) / 2; } }
}

public readonly struct Triangle : IPolygon
{
    public Triangle(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        P1 = p1;
        P2 = p2;
        P3 = p3;
    }
    public readonly Vector2 P1;
    public readonly Vector2 P2;
    public readonly Vector2 P3;
    public readonly Vector2[] Points { get { return [P1, P2, P3]; } }
    public readonly Vector2 Center { get { return (P1 + P2 + P3) / 3; } }
    public static Triangle Clockwise(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float slope1 = (p2.Y - p1.Y) * (p3.X - p2.X);
        float slope2 = (p3.Y - p2.Y) * (p2.X - p1.X);

        // Check orientation
        if (slope1 == slope2)
        {
            throw new Exception("Points are co-linear");
        }
        else if (slope1 < slope2)
        {
            return new Triangle(p1, p3, p2);
        }
        else
        {
            return new Triangle(p1, p2, p3);
        }
    }
    public static Triangle Clockwise(Triangle triangle) => Clockwise(triangle.P1, triangle.P2, triangle.P3);
}

public readonly struct Rectangle : IPolygon
{
    public Rectangle(Vector2 center, Vector2 dimensions)
    {
        Center = center;
        Dimensions = dimensions;
        Rotation = 0;
    }
    public Rectangle(Vector2 center, Vector2 dimensions, Radian rotation)
    {
        Center = center;
        Dimensions = dimensions;
        Rotation = rotation;
    }
    public readonly Vector2 Center { get; }
    public readonly Vector2 Dimensions;
    public readonly Radian Rotation;
    public Vector2[] Points
    {
        get
        {
            Vector2 delta = Dimensions / 2;
            Vector2 TopLeft = Center - delta;
            Vector2 TopRight = Center + new Vector2(delta.X, -delta.Y);
            Vector2 BottomRight = Center + delta;
            Vector2 BottomLeft = Center + new Vector2(-delta.X, delta.Y);
            return [TopLeft, TopRight, BottomRight, BottomLeft];
        }
    }
}