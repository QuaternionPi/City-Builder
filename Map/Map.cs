using CityBuilder.Geometry;
using CityBuilder.Numerics;
using CityBuilder.IO;

namespace CityBuilder.Map;

public class Map
{
    public Camera Camera { get { return CameraMount.Camera; } }
    public CameraMount CameraMount { get; }
    public IEnumerable<Tile> Tiles { get { return from cell in Cells from tile in cell.Tiles select tile; } }
    public readonly Cell[,] Cells;
    public List<Road> Roads { get; protected set; }
    private IMapMode Mode;
    public Map(Cell[,] cells, List<Road> roads)
    {
        float width = cells.GetLength(0) * 2;
        float height = cells.GetLength(1) * 2;
        var zoom = 5;
        var speed = 4;
        CameraMount = new CameraMount(Vector2.Zero, width, height, zoom, speed);
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
public interface IMapDraw
{
    public void Draw(IGraphics graphics);
    public virtual void DrawLabel(IGraphics graphics, Vector2 position) { }
}
public class MapDrawLand : IMapDraw
{
    private readonly Map Map;
    public MapDrawLand(Map map)
    {
        Map = map;
    }
    public void Draw(IGraphics graphics)
    {
        graphics.BeginMode2D(Map.Camera);
        foreach (var tile in Map.Tiles) tile.DrawLand(graphics);
        foreach (var tile in Map.Tiles) tile.DrawStructures(graphics);
        foreach (var road in Map.Roads) road.Draw(graphics);
        graphics.EndMode2D();
    }
    public void DrawLabel(IGraphics graphics, Vector2 position)
    {
        foreach (var tile in Map.Tiles) tile.DrawStructureLabel(graphics, position);
    }
}
public class MapDrawZone : IMapDraw
{
    private readonly Map Map;
    public MapDrawZone(Map map)
    {
        Map = map;
    }
    public void Draw(IGraphics graphics)
    {
        graphics.BeginMode2D(Map.Camera);
        foreach (var tile in Map.Tiles) tile.DrawZone(graphics);
        foreach (var road in Map.Roads) road.Draw(graphics);
        graphics.EndMode2D();
    }
}
public class MapPaint : IMapMode
{
    private readonly Map Map;
    private IMapDraw MapDraw;
    private readonly List<TextButton> Buttons;
    private Land Land;
    private Vector2 LabelPosition;
    public MapPaint(Map map)
    {
        Land = Land.Ocean;
        MapDraw = new MapDrawLand(map);
        Map = map;
        Label transparent = new Label()
            .SetColors(Color.Black, new Color(220, 220, 220, 127));
        Label solid = new Label()
            .SetColors(Color.Black, new Color(220, 220, 220, 255));
        Buttons = [
            new TextButton(
                transparent
                    .SetShape(new Rectangle(new Vector2(60, 20), new Vector2(100, 20)))
                    .SetText("Change View", 40),
                solid
                    .SetShape(new Rectangle(new Vector2(60, 20), new Vector2(100, 20)))
                    .SetText("Change View", 40)
            ),
            new TextButton(
                transparent
                    .SetShape(new Rectangle(new Vector2(60, 50), new Vector2(100, 20)))
                    .SetText("Paint Ocean", 40),
                solid
                    .SetShape(new Rectangle(new Vector2(60, 50), new Vector2(100, 20)))
                    .SetText("Paint Ocean", 40)
            ),
            new TextButton(
                transparent
                    .SetShape(new Rectangle(new Vector2(60, 80), new Vector2(100, 20)))
                    .SetText("Paint Grass", 40),
               solid
                    .SetShape(new Rectangle(new Vector2(60, 80), new Vector2(100, 20)))
                    .SetText("Paint Grass", 40)
            ),
            new TextButton(
                transparent
                    .SetShape(new Rectangle(new Vector2(60, 110), new Vector2(100, 20)))
                    .SetText("Paint Forest", 40),
                solid
                    .SetShape(new Rectangle(new Vector2(60, 110), new Vector2(100, 20)))
                    .SetText("Paint Forest", 40)
            )];
        Buttons[0].Clicked += ChangeView;
        Buttons[1].Clicked += (MouseButton button) => Land = Land.Ocean;
        Buttons[2].Clicked += (MouseButton button) => Land = Land.Grass;
        Buttons[3].Clicked += (MouseButton button) => Land = Land.Forest;
    }
    void ChangeView(MouseButton button)
    {
        if (MapDraw is MapDrawZone)
        {
            MapDraw = new MapDrawLand(Map);
        }
        else
        {
            MapDraw = new MapDrawZone(Map);
        }
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
        LabelPosition = mouse.Position;
        foreach (TextButton button in Buttons)
            (keyboard, mouse) = button.Update(keyboard, mouse, deltaTime);

        var screenToWorld = Map.Camera.GetScreenToWorld2D;
        var worldToScreen = Map.Camera.GetWorldToScreen2D;
        IMouse cameraMouse = new MousePositionTransform(mouse, screenToWorld, worldToScreen);
        foreach (var tile in Map.Tiles)
            (keyboard, cameraMouse) = tile.Update(keyboard, cameraMouse, deltaTime);

        if (keyboard.IsKeyDown(KeyboardKey.W))
        {
            Map.CameraMount.Position -= Vector2.UnitY;
        }
        if (keyboard.IsKeyDown(KeyboardKey.S))
        {
            Map.CameraMount.Position += Vector2.UnitY;
        }
        if (keyboard.IsKeyDown(KeyboardKey.A))
        {
            Map.CameraMount.Position -= Vector2.UnitX;
        }
        if (keyboard.IsKeyDown(KeyboardKey.D))
        {
            Map.CameraMount.Position += Vector2.UnitX;
        }

        Land land = Land;
        if (mouse.IsButtonDown(MouseButton.Left))
            Paint(mouse.Position, land);
        Map.CameraMount.Update(deltaTime);
    }
    public void Draw(IGraphics graphics)
    {
        MapDraw.Draw(graphics);
        MapDraw.DrawLabel(graphics, LabelPosition);
        foreach (var button in Buttons)
        {
            button.Draw(graphics);
        }
    }
}