using CityBuilder.Numerics;
using CityBuilder.Geometry;
using CityBuilder.IO;

namespace CityBuilder.Map;

public class Road
{
    private const int Size = 5;
    public Road(Vector2[] points)
    {
        Name = "temp";
        Points = points;
    }
    public Vector2[] Points { get; protected set; }
    public IEnumerable<Line> Lines
    {
        get
        {
            return
                from (Vector2, Vector2) pair in Points.SkipLast(1).Zip(Points.Skip(1))
                select new Line(pair.Item1, pair.Item2);
        }
    }
    public string Name { get; protected set; }
    public void Draw(IGraphics graphics)
    {
        foreach (Line line in Lines)
        {
            graphics.Line(line, Size, Color.Black);
        }
    }
}