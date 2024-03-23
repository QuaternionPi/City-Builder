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
    public static Color LightGray { get { return new Color(200, 200, 200, 255); } }
    public static Color Gray { get { return new Color(130, 130, 130, 255); } }
    public static Color DarkGray { get { return new Color(80, 80, 80, 255); } }
    public static Color Yellow { get { return new Color(253, 249, 0, 255); } }
    public static Color Gold { get { return new Color(255, 203, 0, 255); } }
    public static Color Orange { get { return new Color(255, 161, 0, 255); } }
    public static Color Pink { get { return new Color(255, 109, 194, 255); } }
    public static Color Red { get { return new Color(230, 41, 55, 255); } }
    public static Color Maroon { get { return new Color(190, 33, 55, 255); } }
    public static Color Green { get { return new Color(0, 228, 48, 255); } }
    public static Color Line { get { return new Color(0, 158, 47, 255); } }
    public static Color DarkGreen { get { return new Color(0, 117, 44, 255); } }
    public static Color SkyBlue { get { return new Color(102, 191, 255, 255); } }
    public static Color Blue { get { return new Color(0, 121, 241, 255); } }
    public static Color DarkBlue { get { return new Color(0, 82, 172, 255); } }
    public static Color Purple { get { return new Color(200, 122, 255, 255); } }
    public static Color Violet { get { return new Color(135, 60, 190, 255); } }
    public static Color DarkPurple { get { return new Color(112, 31, 126, 255); } }
    public static Color Beige { get { return new Color(211, 176, 131, 255); } }
    public static Color Brown { get { return new Color(127, 106, 79, 255); } }
    public static Color DarkBrown { get { return new Color(76, 63, 47, 255); } }
    public static Color White { get { return new Color(255, 255, 255, 255); } }
    public static Color Black { get { return new Color(0, 0, 0, 255); } }
    public static Color Blank { get { return new Color(0, 0, 0, 0); } }
    public static Color Magenta { get { return new Color(255, 0, 255, 255); } }
    public static Color RayWhite { get { return new Color(245, 245, 245, 255); } }
}