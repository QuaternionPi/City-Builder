using CityBuilder.Numerics;
using CityBuilder.IO;

namespace CityBuilder.Map.Structures;

public interface IStructure
{
    public Vector2 Position { get; set; }
    public (IKeyboard, IMouse) Update(IKeyboard keyboard, IMouse mouse, float deltaTime);
    public void Draw(IGraphics graphics);
    public void DrawLabel(IGraphics graphics, Vector2 position);
}