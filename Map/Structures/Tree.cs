using CityBuilder.Numerics;
using CityBuilder.Geometry;
using CityBuilder.IO;

namespace CityBuilder.Map.Structures;
class Tree : IStructure
{
    public Color Color { get; set; }
    public Vector2 Position { get; set; }
    private bool DrawLabelNextFrame;
    private Collider Collider { get { return new Collider(Leaves); } }
    public Triangle Leaves
    {
        get
        {
            return new Triangle(
                new Vector2(-0.4, 0.4) + Position,
                new Vector2(0.4, 0.4) + Position,
                new Vector2(0, -1) + Position
            );
        }
    }
    public Tree()
    {
        Color = Color.DarkGreen;
    }
    public (IKeyboard, IMouse) Update(IKeyboard keyboard, IMouse mouse, float deltaTime)
    {
        DrawLabelNextFrame = Collides(mouse.Position);
        return (keyboard, mouse);
    }
    public void Draw(IGraphics graphics)
    {
        graphics.Triangle(Leaves, Color);
    }
    public void DrawLabel(IGraphics graphics, Vector2 position)
    {
        if (DrawLabelNextFrame)
        {
            var dimensions = new Vector2(100, 20);
            Label label = new Label()
                .SetText("Tree", 12)
                .SetShape(new Rectangle(position + dimensions / 2, dimensions))
                .SetColors(Color.Black, new Color(220, 220, 220, 255));
            label.Draw(graphics);
        }
    }
    protected bool Collides(Vector2 position) => Collider.Collidies(Collider, position);
}