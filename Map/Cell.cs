using CityBuilder.Geometry;
using CityBuilder.IO;

namespace CityBuilder.Map;

public class Cell
{
    private const int Size = 5;
    public Cell(int col, int row, Color[] color)
    {
        Center = (new Vector2(col, row) * 2 + Vector2.One) * Size;
        Dimensions = new Vector2(2, 2) * Size;
        float delta = Size;
        Vector2 topLeft = Center + new Vector2(-delta, -delta);
        Vector2 topRight = Center + new Vector2(delta, -delta);
        Vector2 bottomLeft = Center + new Vector2(-delta, delta);
        Vector2 bottomRight = Center + new Vector2(delta, delta);

        Triangle top = Triangle.Clockwise(Center, topLeft, topRight);
        Triangle right = Triangle.Clockwise(Center, topRight, bottomRight);
        Triangle bottom = Triangle.Clockwise(Center, bottomLeft, bottomRight);
        Triangle left = Triangle.Clockwise(Center, topLeft, bottomLeft);

        Top = new Tile(top, new Land(color[0], false));
        Right = new Tile(right, new Land(color[1], false));
        Bottom = new Tile(bottom, new Land(color[2], false));
        Left = new Tile(left, new Land(color[3], false));
    }
    private readonly Vector2 Center;
    private readonly Vector2 Dimensions;
    public readonly Tile Top;
    public readonly Tile Right;
    public readonly Tile Bottom;
    public readonly Tile Left;
    public Tile[] Tiles { get { return new Tile[] { Top, Right, Bottom, Left }; } }
    public bool Collidies(Collider collider) => Collider.Collidies(new Collider(new Rectangle(Center, Dimensions)), collider);
    public bool Collidies(Vector2 point) => Collider.Collidies(new Collider(new Rectangle(Center, Dimensions)), point);
    public void Draw(IGraphics graphics)
    {
    }
}