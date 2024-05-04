using CityBuilder.Numerics;
using Raylib_cs;

namespace CityBuilder.IO;

public static class Window
{
    public static void Init(int width, int height, string title) =>
        Raylib.InitWindow(width, height, title);
    public static void Close() => Raylib.CloseWindow();
    public static bool ShouldClose() => Raylib.WindowShouldClose();
    public static bool IsReady() => Raylib.IsWindowReady();
    public static bool IsFullscreen() => Raylib.IsWindowFullscreen();
    public static bool IsHidden() => Raylib.IsWindowHidden();
    public static bool IsMinimized() => Raylib.IsWindowMinimized();
    public static bool IsMaximized() => Raylib.IsWindowMaximized();
    public static bool IsFocused() => Raylib.IsWindowFocused();
    public static bool IsResized() => Raylib.IsWindowResized();
    public static bool IsState(ConfigFlags flag) => Raylib.IsWindowState(flag);
    public static void SetState(ConfigFlags flag) => Raylib.SetConfigFlags(flag);
    public static void SetConfigFlags(ConfigFlags flag) => Raylib.SetConfigFlags(flag);
    public static void ClearState(ConfigFlags flag) => Raylib.ClearWindowState(flag);
    public static void ToggleFullScreen() => Raylib.ToggleFullscreen();
    public static void Maximize() => Raylib.MaximizeWindow();
    public static void Minimize() => Raylib.MinimizeWindow();
    public static void Restore() => Raylib.RestoreWindow();
    public static void SetWindowIcon(Image image) => Raylib.SetWindowIcon(image);
    public static void SetTitle(string title) => Raylib.SetWindowTitle(title);
    public static void SetPosition(int x, int y) => Raylib.SetWindowPosition(x, y);
    public static void SetMonitor(int monitor) => Raylib.SetWindowMonitor(monitor);
    public static void SetMinimumSize(int width, int height) => Raylib.SetWindowMinSize(width, height);
    public static void SetSize(int width, int height) => Raylib.SetWindowSize(width, height);
    public static void SetOpacity(float opacity) => Raylib.SetWindowOpacity(opacity);
    public static Vector2 GetDimensions() => new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
    public static Vector2 GetPosition() => Raylib.GetWindowPosition();
    public static void SetTargetFPS(int fps) => Raylib.SetTargetFPS(fps);
    public static float GetFrameTime() => Raylib.GetFrameTime();
    public static double GetTime() => Raylib.GetTime();
    public static int GetFPS() => Raylib.GetFPS();
}