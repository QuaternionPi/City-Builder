
using CityBuilder.Geometry;
using CityBuilder.IO;

namespace CityBuilder.Map.Structures;
class Tree : IStructure
{
    public Tree() { }
    public Vector2 Position { get; set; }
    public (IKeyboard, IMouse) Update(IKeyboard keyboard, IMouse mouse, float deltaTime)
    {
        return (keyboard, mouse);
    }
    public void Draw(IGraphics graphics)
    {
        graphics.Circle(new Circle(Position, 3), Color.Violet);
    }
}