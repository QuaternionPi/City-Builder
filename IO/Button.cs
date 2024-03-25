using CityBuilder.Geometry;

namespace CityBuilder.IO;

public class TextButton
{
    private Label Normal;
    private Label MouseOver;
    private bool MousedOver;
    private Label ActivePrototype { get { return MousedOver ? MouseOver : Normal; } }
    public delegate void Click(MouseButton button);
    public event Click? Clicked;
    public TextButton(Label normal, Label mouseOver)
    {
        Normal = normal;
        MouseOver = mouseOver;
    }
    public (IKeyboard, IMouse) Update(IKeyboard keyboard, IMouse mouse, float deltaTime)
    {
        MousedOver = Collides(mouse.Position);
        if (MousedOver == false) return (keyboard, mouse);

        if (mouse.IsButtonReleased(MouseButton.Left)) { Clicked?.Invoke(MouseButton.Left); return (keyboard, mouse.Handle(MouseButton.Left)); }
        if (mouse.IsButtonReleased(MouseButton.Right)) { Clicked?.Invoke(MouseButton.Right); return (keyboard, mouse.Handle(MouseButton.Right)); }

        return (keyboard, mouse.Handle(MouseButton.Left).Handle(MouseButton.Right));
    }
    public void Draw(IGraphics graphics)
    {
        ActivePrototype.Draw(graphics);
    }
    protected bool Collides(Vector2 position) => ActivePrototype.Collides(position);
}