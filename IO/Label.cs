using CityBuilder.Numerics;
using CityBuilder.Geometry;

namespace CityBuilder.IO;

public readonly struct Label
{
    private readonly Color TextColor;
    private readonly Color BackgroundColor;
    private readonly Rectangle Shape;
    private readonly Collider Collider;
    private readonly string Text;
    private readonly int Size;
    private readonly Font Font;
    public Label()
    {
        TextColor = Color.Black;
        BackgroundColor = Color.RayWhite;
        Text = "";
        Size = 10;
        Shape = new Rectangle(Vector2.Zero, Vector2.Zero);
        Collider = new Collider(Shape);
        Font = Font.DefaultFont();
    }
    private Label(Color textColor, Color backgroundColor, Label prototype)
    {
        TextColor = textColor;
        BackgroundColor = backgroundColor;
        Text = prototype.Text;
        Size = prototype.Size;
        Shape = prototype.Shape;
        Collider = new Collider(Shape);
        Font = prototype.Font;
    }
    private Label(string text, int fontSize, Label prototype)
    {
        TextColor = prototype.TextColor;
        BackgroundColor = prototype.BackgroundColor;
        Text = text;
        Size = prototype.Size;
        Shape = prototype.Shape;
        Collider = new Collider(Shape);
        Font = prototype.Font;
    }
    private Label(Rectangle shape, Label prototype)
    {
        TextColor = prototype.TextColor;
        BackgroundColor = prototype.BackgroundColor;
        Text = prototype.Text;
        Size = prototype.Size;
        Shape = shape;
        Collider = new Collider(Shape);
        Font = prototype.Font;
    }
    private Label(Font font, int fontSize, Label prototype)
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
    public Label SetColors(Color textColor, Color backgroundColor) => new Label(textColor, backgroundColor, this);
    public Label SetText(string text, int fontSize) => new Label(text, fontSize, this);
    public Label SetShape(Rectangle shape) => new Label(shape, this);
    public Label SetFont(Font font, int fontSize) => new Label(font, fontSize, this);
}