using CityBuilder.Numerics;

namespace CityBuilder.IO;

public static class Window
{
    public static void Init(int width, int height, string title) =>
        Raylib_cs.Raylib.InitWindow(width, height, title);
    public static void Close() => Raylib_cs.Raylib.CloseWindow();
    public static bool ShouldClose() => Raylib_cs.Raylib.WindowShouldClose();
    public static bool IsReady() => Raylib_cs.Raylib.IsWindowReady();
    public static bool IsFullscreen() => Raylib_cs.Raylib.IsWindowFullscreen();
    public static bool IsHidden() => Raylib_cs.Raylib.IsWindowHidden();
    public static bool IsMinimized() => Raylib_cs.Raylib.IsWindowMinimized();
    public static bool IsMaximized() => Raylib_cs.Raylib.IsWindowMaximized();
    public static bool IsFocused() => Raylib_cs.Raylib.IsWindowFocused();
    public static bool IsResized() => Raylib_cs.Raylib.IsWindowResized();
    public static bool IsState(Raylib_cs.ConfigFlags flag) => Raylib_cs.Raylib.IsWindowState(flag);
    public static void SetState(Raylib_cs.ConfigFlags flag) => Raylib_cs.Raylib.SetConfigFlags(flag);
    public static void SetConfigFlags(Raylib_cs.ConfigFlags flag) => Raylib_cs.Raylib.SetConfigFlags(flag);
    public static void ClearState(Raylib_cs.ConfigFlags flag) => Raylib_cs.Raylib.ClearWindowState(flag);
    public static void ToggleFullScreen() => Raylib_cs.Raylib.ToggleFullscreen();
    public static void Maximize() => Raylib_cs.Raylib.MaximizeWindow();
    public static void Minimize() => Raylib_cs.Raylib.MinimizeWindow();
    public static void Restore() => Raylib_cs.Raylib.RestoreWindow();
    public static void SetWindowIcon(Raylib_cs.Image image) => Raylib_cs.Raylib.SetWindowIcon(image);
    public static void SetTitle(string title) => Raylib_cs.Raylib.SetWindowTitle(title);
    public static void SetPosition(int x, int y) => Raylib_cs.Raylib.SetWindowPosition(x, y);
    public static void SetMonitor(int monitor) => Raylib_cs.Raylib.SetWindowMonitor(monitor);
    public static void SetMinimumSize(int width, int height) => Raylib_cs.Raylib.SetWindowMinSize(width, height);
    public static void SetSize(int width, int height) => Raylib_cs.Raylib.SetWindowSize(width, height);
    public static void SetOpacity(float opacity) => Raylib_cs.Raylib.SetWindowOpacity(opacity);
    public static Vector2 GetDimensions() => new Vector2(Raylib_cs.Raylib.GetScreenWidth(), Raylib_cs.Raylib.GetScreenHeight());
    public static Vector2 GetPosition() => Raylib_cs.Raylib.GetWindowPosition();
    public static void SetTargetFPS(int fps) => Raylib_cs.Raylib.SetTargetFPS(fps);
    public static float GetFrameTime() => Raylib_cs.Raylib.GetFrameTime();
    public static double GetTime() => Raylib_cs.Raylib.GetTime();
    public static int GetFPS() => Raylib_cs.Raylib.GetFPS();
}