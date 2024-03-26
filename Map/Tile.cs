using CityBuilder.Numerics;
using CityBuilder.Geometry;
using CityBuilder.Map.Structures;
using CityBuilder.IO;

namespace CityBuilder.Map;

public class Tile
{
    private readonly Triangle Triangle;
    private Land Land;
    private Zone? Zone;
    private List<IStructure> Structures;
    public Tile(Triangle triangle, Land land, Zone? zone, IEnumerable<IStructure> structures)
    {
        Triangle = triangle;
        Land = land;
        Zone = zone;
        Structures = structures.ToList();
        foreach (IStructure structure in structures)
            structure.Position = Triangle.Center;
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
    public (IKeyboard, IMouse) Update(IKeyboard keyboard, IMouse mouse, float deltaTime)
    {
        foreach (var structure in Structures)
            (keyboard, mouse) = structure.Update(keyboard, mouse, deltaTime);
        return (keyboard, mouse);
    }
    public void DrawLand(IGraphics graphics)
    {
        graphics.Triangle(Triangle, Land.Color);
    }
    public void DrawStructures(IGraphics graphics)
    {
        foreach (var structure in Structures)
            structure.Draw(graphics);
    }
    public void DrawStructureLabel(IGraphics graphics, Vector2 position)
    {
        foreach (var structure in Structures)
            structure.DrawLabel(graphics, position);
    }
    public void DrawZone(IGraphics graphics)
    {
        Color color;
        if (Zone != null) color = Zone.Color;
        else color = Color.Gray;
        graphics.Triangle(Triangle, color);
    }
}