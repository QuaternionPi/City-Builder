using CityBuilder.Geometry;
using CityBuilder.IO;

namespace CityBuilder.Map;

public class Cell
{
    private const int Size = 5;
    public Cell(int col, int row, Land[] lands, Zone?[] zones)
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

        Top = new Tile(top, lands[0], zones[0]);
        Right = new Tile(right, lands[1], zones[1]);
        Bottom = new Tile(bottom, lands[2], zones[2]);
        Left = new Tile(left, lands[3], zones[3]);
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