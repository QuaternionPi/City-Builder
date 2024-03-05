namespace CityBuilder;

public readonly struct Color
{
    public static implicit operator Raylib_cs.Color(Color color) => new Raylib_cs.Color(color.R, color.G, color.B, color.A);
    public Color(byte r, byte g, byte b, byte a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }
    public readonly byte R;
    public readonly byte G;
    public readonly byte B;
    public readonly byte A;
    public static Color LIGHTGRAY { get { return new Color(200, 200, 200, 255); } }
    public static Color GRAY { get { return new Color(130, 130, 130, 255); } }
    public static Color DARKGRAY { get { return new Color(80, 80, 80, 255); } }
    public static Color YELLOW { get { return new Color(253, 249, 0, 255); } }
    public static Color GOLD { get { return new Color(255, 203, 0, 255); } }
    public static Color ORANGE { get { return new Color(255, 161, 0, 255); } }
    public static Color PINK { get { return new Color(255, 109, 194, 255); } }
    public static Color RED { get { return new Color(230, 41, 55, 255); } }
    public static Color MAROON { get { return new Color(190, 33, 55, 255); } }
    public static Color GREEN { get { return new Color(0, 228, 48, 255); } }
    public static Color LIME { get { return new Color(0, 158, 47, 255); } }
    public static Color DARKGREEN { get { return new Color(0, 117, 44, 255); } }
    public static Color SKYBLUE { get { return new Color(102, 191, 255, 255); } }
    public static Color BLUE { get { return new Color(0, 121, 241, 255); } }
    public static Color DARKBLUE { get { return new Color(0, 82, 172, 255); } }
    public static Color PURPLE { get { return new Color(200, 122, 255, 255); } }
    public static Color VIOLET { get { return new Color(135, 60, 190, 255); } }
    public static Color DARKPURPLE { get { return new Color(112, 31, 126, 255); } }
    public static Color BEIGE { get { return new Color(211, 176, 131, 255); } }
    public static Color BROWN { get { return new Color(127, 106, 79, 255); } }
    public static Color DARKBROWN { get { return new Color(76, 63, 47, 255); } }
    public static Color WHITE { get { return new Color(255, 255, 255, 255); } }
    public static Color BLACK { get { return new Color(0, 0, 0, 255); } }
    public static Color BLANK { get { return new Color(0, 0, 0, 0); } }
    public static Color MAGENTA { get { return new Color(255, 0, 255, 255); } }
    public static Color RAYWHITE { get { return new Color(245, 245, 245, 255); } }
}