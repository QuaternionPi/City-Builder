using CityBuilder.Geometry;

namespace CityBuilder.IO;

public class TextButton
{
    private Prototype Normal;
    private Prototype MouseOver;
    private bool MousedOver;
    private Prototype ActivePrototype { get { return MousedOver ? MouseOver : Normal; } }
    public delegate void Click(MouseButton button);
    public event Click? Clicked;
    public TextButton(Prototype normal, Prototype mouseOver)
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
    public readonly struct Prototype
    {
        private readonly Color TextColor;
        private readonly Color BackgroundColor;
        private readonly Rectangle Shape;
        private readonly Collider Collider;
        private readonly string Text;
        private readonly int Size;
        private readonly Font Font;
        public Prototype()
        {
            TextColor = Color.Black;
            BackgroundColor = Color.RayWhite;
            Text = "";
            Size = 10;
            Shape = new Rectangle(Vector2.Zero, Vector2.Zero);
            Collider = new Collider(Shape);
            Font = Font.DefaultFont();
        }
        private Prototype(Color textColor, Color backgroundColor, Prototype prototype)
        {
            TextColor = textColor;
            BackgroundColor = backgroundColor;
            Text = prototype.Text;
            Size = prototype.Size;
            Shape = prototype.Shape;
            Collider = new Collider(Shape);
            Font = prototype.Font;
        }
        private Prototype(string text, int fontSize, Prototype prototype)
        {
            TextColor = prototype.TextColor;
            BackgroundColor = prototype.BackgroundColor;
            Text = text;
            Size = prototype.Size;
            Shape = prototype.Shape;
            Collider = new Collider(Shape);
            Font = prototype.Font;
        }
        private Prototype(Rectangle shape, Prototype prototype)
        {
            TextColor = prototype.TextColor;
            BackgroundColor = prototype.BackgroundColor;
            Text = prototype.Text;
            Size = prototype.Size;
            Shape = shape;
            Collider = new Collider(Shape);
            Font = prototype.Font;
        }
        private Prototype(Font font, int fontSize, Prototype prototype)
        {
            TextColor = prototype.TextColor;
            BackgroundColor = prototype.BackgroundColor;
            Text = prototype.Text;
            Size = fontSize;
            Shape = prototype.Shape;
            Collider = new Collider(Shape);
            Font = font;
        }
        public bool Collides(Vector2 position) => Collider.Collidies(Collider, position);
        public void Draw(IGraphics graphics)
        {
            graphics.Rectangle(Shape, BackgroundColor);
            graphics.Text(Font, Text, Shape.Center, 0, Size, 1, TextColor);
        }
        public Prototype SetColors(Color textColor, Color backgroundColor) => new Prototype(textColor, backgroundColor, this);
        public Prototype SetText(string text, int fontSize) => new Prototype(text, fontSize, this);
        public Prototype SetShape(Rectangle shape) => new Prototype(shape, this);
        public Prototype SetFont(Font font, int fontSize) => new Prototype(font, fontSize, this);
    }
}