using CityBuilder.Geometry;
using CityBuilder.IO;

namespace CityBuilder.Map;

public class Tile
{
    public Tile(Triangle triangle, Color color)
    {
        Triangle = triangle;
        Color = color;
    }
    private readonly Triangle Triangle;
    private Color Color;
    public void Paint(Color color)
    {
        Color = color;
    }
    public bool Collidies(Collider collider) => Collider.Collidies(new Collider(Triangle), collider);
    public bool Collidies(Vector2 point) => Collider.Collidies(new Collider(Triangle), point);
    public void Draw(IGraphics graphics)
    {
        graphics.Triangle(Triangle, Color);
    }
}