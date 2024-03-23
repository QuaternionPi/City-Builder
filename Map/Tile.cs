using CityBuilder.Geometry;
using CityBuilder.IO;

namespace CityBuilder.Map;

public class Tile
{
    private readonly Triangle Triangle;
    private Land Land;
    private Zone? Zone;
    public Tile(Triangle triangle, Land land, Zone? zone = null)
    {
        Triangle = triangle;
        Land = land;
        Zone = zone;
    }
    public void Paint(Land land)
    {
        Land = land;
    }
    public void Paint(Zone zone)
    {
        Zone = zone;
    }
    public bool Collidies(Collider collider) => Collider.Collidies(new Collider(Triangle), collider);
    public bool Collidies(Vector2 point) => Collider.Collidies(new Collider(Triangle), point);
    public void DrawLand(IGraphics graphics)
    {
        graphics.Triangle(Triangle, Land.Color);
    }
    public void DrawZone(IGraphics graphics)
    {
        Color color;
        if (Zone != null) color = Zone.Color;
        else color = Color.Gray;
        graphics.Triangle(Triangle, color);
    }
}