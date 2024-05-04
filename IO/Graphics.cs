using CityBuilder.Numerics;
using CityBuilder.Geometry;

namespace CityBuilder.IO;

using SystemVector2 = System.Numerics.Vector2;

public interface IGraphics
{
    void ClearBackground(Color color);
    void BeginDrawing();
    void EndDrawing();
    void BeginMode2D(Camera camera);
    void EndMode2D();
    void BeginTextureMode(Raylib_cs.RenderTexture2D target);
    void EndTextureMode();
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
    public void ClearBackground(Color color) => Raylib_cs.Raylib.ClearBackground(color);
    public void BeginDrawing() => Raylib_cs.Raylib.BeginDrawing();
    public void EndDrawing() => Raylib_cs.Raylib.EndDrawing();
    public void BeginMode2D(Camera camera) => Raylib_cs.Raylib.BeginMode2D((Raylib_cs.Camera2D)camera);
    public void EndMode2D() => Raylib_cs.Raylib.EndMode2D();
    public void BeginTextureMode(Raylib_cs.RenderTexture2D target) => Raylib_cs.Raylib.BeginTextureMode(target);
    public void EndTextureMode() => Raylib_cs.Raylib.EndTextureMode();
    public Vector2 GetScreenToWorld2D(Vector2 position, Camera camera) => (Vector2)Raylib_cs.Raylib.GetScreenToWorld2D((SystemVector2)position, (Raylib_cs.Camera2D)camera);
    public Vector2 GetWorldToScreen2D(Vector2 position, Camera camera) => (Vector2)Raylib_cs.Raylib.GetWorldToScreen2D((SystemVector2)position, (Raylib_cs.Camera2D)camera);
    public void Pixel(Vector2 position, Color color) => Raylib_cs.Raylib.DrawPixelV((SystemVector2)position, color);
    public void Line(Line line, float thick, Color color) => Raylib_cs.Raylib.DrawLineEx((SystemVector2)line.Start, (SystemVector2)line.End, thick, color);
    public void Bezier(Line line, float thick, Color color) => Raylib_cs.Raylib.DrawLineBezier((SystemVector2)line.Start, (SystemVector2)line.End, thick, color);
    public void Circle(Circle circle, Color color) => Raylib_cs.Raylib.DrawCircle((int)circle.Center.X, (int)circle.Center.Y, circle.Radius, color);
    public void Sector(Sector sector, Color color) =>
        Raylib_cs.Raylib.DrawCircleSector((SystemVector2)sector.Center, sector.Radius, (float)(Degree)sector.Start, (float)(Degree)sector.End, 100, color);
    public void Ring(Ring ring, Color color) => Raylib_cs.Raylib.DrawRing((SystemVector2)ring.Center, ring.RadiusInner, ring.RadiusOuter, (float)(Degree)ring.Start, (float)(Degree)ring.End, 100, color);
    public void Triangle(Triangle triangle, Color color) => Raylib_cs.Raylib.DrawTriangle((SystemVector2)triangle.P1, (SystemVector2)triangle.P2, (SystemVector2)triangle.P3, color);
    public void Rectangle(Rectangle rectangle, Color color)
    {
        Vector2 position = rectangle.Center - rectangle.Dimensions / 2;
        Raylib_cs.Rectangle raylibRec = new Raylib_cs.Rectangle(position.X, position.Y, rectangle.Dimensions.X, rectangle.Dimensions.Y);
        Raylib_cs.Raylib.DrawRectanglePro(raylibRec, SystemVector2.Zero, (float)(Degree)rectangle.Rotation, color);
    }
    public void Text(Font font, string text, Vector2 position, Radian rotation, float fontSize, float spacing, Color color) =>
        Raylib_cs.Raylib.DrawTextPro(font, text, (SystemVector2)(position - font.MeasureText(text, fontSize, spacing) / 2), SystemVector2.Zero, (float)(Degree)rotation, fontSize, spacing, color);
}