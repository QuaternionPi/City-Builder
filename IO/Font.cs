using CityBuilder.Numerics;

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
        return (Vector2)Raylib_cs.Raylib.MeasureTextEx(Value, text, fontSize, spacing);
    }
    public static Font DefaultFont() => new Font(Raylib_cs.Raylib.GetFontDefault());
}