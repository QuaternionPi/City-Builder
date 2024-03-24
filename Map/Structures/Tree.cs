using CityBuilder.Geometry;
using CityBuilder.IO;

namespace CityBuilder.Map.Structures;
class Tree : IStructure
{
    public Color Color { get; set; }
    public Triangle Leaves
    {
        get
        {
            return new Triangle(
                new Vector2(-1.5, 1.5) + Position,
                new Vector2(1.5, 1.5) + Position,
                new Vector2(0, -2) + Position
            );
        }
    }
    public Vector2 Position { get; set; }
    public Tree()
    {
        Color = Color.DarkPurple;
    }
    public (IKeyboard, IMouse) Update(IKeyboard keyboard, IMouse mouse, float deltaTime)
    {
        return (keyboard, mouse);
    }
    public void Draw(IGraphics graphics)
    {
        graphics.Triangle(Leaves, Color);
    }
}