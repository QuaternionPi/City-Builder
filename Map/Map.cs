using CityBuilder.Geometry;
using CityBuilder.IO;

namespace CityBuilder.Map;

public class Map
{
    private IMapMode Mode;
    public readonly Cell[,] Cells;
    public IEnumerable<Tile> Tiles { get { return from cell in Cells from tile in cell.Tiles select tile; } }
    public List<Road> Roads { get; protected set; }
    public Map(Cell[,] cells, List<Road> roads)
    {
        Cells = cells;
        Roads = roads;
        Mode = new MapPaint(this);
    }
    public Vector2 Dimensions() => new(Cells.GetLength(0), Cells.GetLength(1));
    public void Update(IKeyboard keyboard, IMouse mouse, float deltaTime) => Mode.Update(keyboard, mouse, deltaTime);
    public void Draw(IGraphics graphics) => Mode.Draw(graphics);
}
public interface IMapMode
{
    public void Update(IKeyboard keyboard, IMouse mouse, float deltaTime);
    public void Draw(IGraphics graphics);
}
public class MapDisplay : IMapMode
{
    private readonly Map Map;
    public MapDisplay(Map map)
    {
        Map = map;
    }
    public void Update(IKeyboard keyboard, IMouse mouse, float deltaTime) { }
    public void Draw(IGraphics graphics)
    {
        foreach (var tile in Map.Tiles) tile.DrawLand(graphics);
        foreach (var road in Map.Roads)
        {
            road.Draw(graphics);
        }
    }
}
public class MapPaint : IMapMode
{
    private readonly Map Map;
    private readonly List<TextButton> Buttons;
    public MapPaint(Map map)
    {
        Map = map;
        Buttons = [ new TextButton(
                new TextButton.Prototype()
                    .SetShape(new Rectangle(new Vector2(200, 100), new Vector2(100, 20)))
                    .SetText("Button", 40)
                    .SetColors(Color.BLACK,new Color(220, 220, 220, 127)),
                new TextButton.Prototype()
                    .SetShape(new Rectangle(new Vector2(200, 100), new Vector2(100, 20)))
                    .SetText("Button", 40)
                    .SetColors(Color.BLACK,new Color(220, 220, 220, 255))
            ) ];
        Buttons[0].Clicked += PrintMessage;
    }
    static void PrintMessage(MouseButton button)
    {
        string message = "message";
        Console.WriteLine(message);
    }
    private void Paint(Vector2 position, Land land)
    {
        var hitCells =
            from Cell cell in Map.Cells
            where cell.Collidies(position)
            select cell;

        if (hitCells.Any() == false) { return; }
        Cell hitCell = hitCells.First();

        var hitTiles =
            from Tile tile in hitCell.Tiles
            where tile.Collidies(position)
            select tile;

        if (hitTiles.Any() == false) { return; }
        Tile hitTile = hitTiles.First();

        hitTile.Paint(land);
    }
    public void Update(IKeyboard keyboard, IMouse mouse, float deltaTime)
    {
        foreach (TextButton button in Buttons)
        {
            (keyboard, mouse) = button.Update(keyboard, mouse, deltaTime);
        }

        Land land = new Land(Color.DARKBLUE, false);
        if (mouse.IsButtonDown(MouseButton.Left))
            Paint(mouse.Position, land);
    }
    public void Draw(IGraphics graphics)
    {
        foreach (var tile in Map.Tiles) tile.DrawLand(graphics);
        foreach (var road in Map.Roads)
        {
            road.Draw(graphics);
        }
        foreach (var button in Buttons)
        {
            button.Draw(graphics);
        }
    }
}