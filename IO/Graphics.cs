using CityBuilder.Numerics;
using CityBuilder.Geometry;

namespace CityBuilder.IO;

public class Font
{
    private Raylib_cs.Font Value;
    public static implicit operator Raylib_cs.Font(CityBuilder.IO.Font font) { return font.Value; }
    private Font(Raylib_cs.Font font)
    {
        Value = font;
    }
    public Vector2 MeasureText(string text, float fontSize, float spacing)
    {
        System.Numerics.Vector2 systemVector = Raylib_cs.Raylib.MeasureTextEx(Value, text, fontSize, spacing);
        return new Vector2(systemVector.X, systemVector.Y);
    }
    public static Font DefaultFont() => new Font(Raylib_cs.Raylib.GetFontDefault());
}

public readonly struct Camera
{
    private static System.Numerics.Vector2 SystemVector(Vector2 vector) => new System.Numerics.Vector2(vector.X, vector.Y);
    public static implicit operator Raylib_cs.Camera2D(Camera camera) => new Raylib_cs.Camera2D(SystemVector(camera.Offset), SystemVector(camera.Target), (float)camera.Rotation, camera.Zoom);
    public readonly Vector2 Offset;
    public readonly Vector2 Target;
    public readonly Degree Rotation;
    public readonly float Zoom;
    public Camera()
    {
        Offset = Vector2.Zero;
        Target = Vector2.Zero;
        Rotation = 0;
        Zoom = 1;
    }
    public Camera(Vector2 offset, Vector2 target, Degree rotation, float zoom)
    {
        Offset = offset;
        Target = target;
        Rotation = rotation;
        Zoom = zoom;
    }
    public Camera(Vector2 offset, Vector2 target, Degree rotation, double zoom)
    {
        Offset = offset;
        Target = target;
        Rotation = rotation;
        Zoom = (float)zoom;
    }
}

public interface IGraphics
{
    void ClearBackground(Color color);
    void BeginDrawing();
    void EndDrawing();
    void BeginMode2D(Camera camera);
    void EndMode2D();
    void BeginTextureMode(Raylib_cs.RenderTexture2D target);
    void EndTextureMode();
    Vector2 GetScreenToWorld2D(Vector2 position, Camera camera);
    Vector2 GetWorldToScreen2D(Vector2 position, Camera camera);
    void Pixel(Vector2 position, Color color);
    void Line(Line line, float thick, Color color);
    void Bezier(Line line, float thick, Color color);
    void Circle(Circle circle, Color color);
    void Sector(Sector sector, Color color);
    void Ring(Ring ring, Color color);
    void Triangle(Triangle triangle, Color color);
    void Rectangle(Geometry.Rectangle rectangle, Color color);
    void Text(Font font, string text, Vector2 position, Radian rotation, float fontSize, float spacing, Color color);
}

public class RaylibGraphics : IGraphics
{
    private static System.Numerics.Vector2 SystemVector(Vector2 vector) => new System.Numerics.Vector2(vector.X, vector.Y);
    private static Vector2 CustomVector(System.Numerics.Vector2 vector) => new Vector2(vector.X, vector.Y);
    public void ClearBackground(Color color) => Raylib_cs.Raylib.ClearBackground(color);
    public void BeginDrawing() => Raylib_cs.Raylib.BeginDrawing();
    public void EndDrawing() => Raylib_cs.Raylib.EndDrawing();
    public void BeginMode2D(Camera camera) => Raylib_cs.Raylib.BeginMode2D((Raylib_cs.Camera2D)camera);
    public void EndMode2D() => Raylib_cs.Raylib.EndMode2D();
    public void BeginTextureMode(Raylib_cs.RenderTexture2D target) => Raylib_cs.Raylib.BeginTextureMode(target);
    public void EndTextureMode() => Raylib_cs.Raylib.EndTextureMode();
    public Vector2 GetScreenToWorld2D(Vector2 position, Camera camera) => CustomVector(Raylib_cs.Raylib.GetScreenToWorld2D(SystemVector(position), (Raylib_cs.Camera2D)camera));
    public Vector2 GetWorldToScreen2D(Vector2 position, Camera camera) => CustomVector(Raylib_cs.Raylib.GetWorldToScreen2D(SystemVector(position), (Raylib_cs.Camera2D)camera));
    public void Pixel(Vector2 position, Color color) => Raylib_cs.Raylib.DrawPixelV(SystemVector(position), color);
    public void Line(Line line, float thick, Color color) => Raylib_cs.Raylib.DrawLineEx(SystemVector(line.Start), SystemVector(line.End), thick, color);
    public void Bezier(Line line, float thick, Color color) => Raylib_cs.Raylib.DrawLineBezier(SystemVector(line.Start), SystemVector(line.End), thick, color);
    public void Circle(Circle circle, Color color) => Raylib_cs.Raylib.DrawCircle((int)circle.Center.X, (int)circle.Center.Y, circle.Radius, color);
    public void Sector(Sector sector, Color color) =>
        Raylib_cs.Raylib.DrawCircleSector(SystemVector(sector.Center), sector.Radius, (float)(Degree)sector.Start, (float)(Degree)sector.End, 100, color);
    public void Ring(Ring ring, Color color) => Raylib_cs.Raylib.DrawRing(SystemVector(ring.Center), ring.RadiusInner, ring.RadiusOuter, (float)(Degree)ring.Start, (float)(Degree)ring.End, 100, color);
    public void Triangle(Triangle triangle, Color color) => Raylib_cs.Raylib.DrawTriangle(SystemVector(triangle.P1), SystemVector(triangle.P2), SystemVector(triangle.P3), color);
    public void Rectangle(Geometry.Rectangle rectangle, Color color)
    {
        Vector2 position = rectangle.Center - rectangle.Dimensions / 2;
        Raylib_cs.Rectangle raylibRec = new Raylib_cs.Rectangle(position.X, position.Y, rectangle.Dimensions.X, rectangle.Dimensions.Y);
        Raylib_cs.Raylib.DrawRectanglePro(raylibRec, System.Numerics.Vector2.Zero, (float)(Degree)rectangle.Rotation, color);
    }
    public void Text(Font font, string text, Vector2 position, Radian rotation, float fontSize, float spacing, Color color) =>
        Raylib_cs.Raylib.DrawTextPro(font, text, SystemVector(position - font.MeasureText(text, fontSize, spacing) / 2), System.Numerics.Vector2.Zero, (float)(Degree)rotation, fontSize, spacing, color);
}