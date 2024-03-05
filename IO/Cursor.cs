using Raylib_cs;

namespace CityBuilder.IO;

public static class Cursor
{
    public static void Show() => Raylib.ShowCursor();
    public static void Hide() => Raylib.HideCursor();
    public static bool IsHidden() => Raylib.IsCursorHidden();
    public static void Enable() => Raylib.EnableCursor();
    public static void Disable() => Raylib.DisableCursor();
    public static bool IsOnScreen() => Raylib.IsCursorOnScreen();
}
