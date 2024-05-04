using CityBuilder.Numerics;

namespace CityBuilder.IO;

public enum MouseButton { Left = 0, Right = 1, Middle = 2, Side = 3, Extra = 4, Forward = 5, Back = 6 }
public interface IMouse
{
    bool IsButtonPressed(MouseButton button);
    bool IsButtonDown(MouseButton button);
    bool IsButtonReleased(MouseButton button);
    bool IsButtonUp(MouseButton button);
    Vector2 Position { get; set; }
    Vector2 GetDelta();
    Vector2 GetMouseWheelMove();
}
public static class MouseExtensions
{
    public static HandleMouseInput Handle(this IMouse mouse, MouseButton button) => new HandleMouseInput(mouse, button);
}
public class RaylibMouse : IMouse
{
    public bool IsButtonPressed(MouseButton button) => Raylib_cs.Raylib.IsMouseButtonPressed((Raylib_cs.MouseButton)button);
    public bool IsButtonDown(MouseButton button) => Raylib_cs.Raylib.IsMouseButtonDown((Raylib_cs.MouseButton)button);
    public bool IsButtonReleased(MouseButton button) => Raylib_cs.Raylib.IsMouseButtonReleased((Raylib_cs.MouseButton)button);
    public bool IsButtonUp(MouseButton button) => Raylib_cs.Raylib.IsMouseButtonUp((Raylib_cs.MouseButton)button);
    public Vector2 Position
    {
        get { return new Vector2(Raylib_cs.Raylib.GetMousePosition().X, Raylib_cs.Raylib.GetMousePosition().Y); }
        set { Raylib_cs.Raylib.SetMousePosition((int)value.X, (int)value.Y); }
    }
    public Vector2 GetDelta() => new Vector2(Raylib_cs.Raylib.GetMouseDelta().X, Raylib_cs.Raylib.GetMouseDelta().Y);
    public Vector2 GetMouseWheelMove() => new Vector2(Raylib_cs.Raylib.GetMouseWheelMoveV().X, Raylib_cs.Raylib.GetMouseWheelMoveV().Y);
}
public class HandleMouseInput : IMouse
{
    private IMouse Mouse;
    private MouseButton Button;
    public HandleMouseInput(IMouse mouse, MouseButton button)
    {
        Mouse = mouse;
        Button = button;
    }
    public bool IsButtonPressed(MouseButton button) => button == Button ? false : Mouse.IsButtonPressed(button);
    public bool IsButtonDown(MouseButton button) => button == Button ? false : Mouse.IsButtonDown(button);
    public bool IsButtonReleased(MouseButton button) => button == Button ? false : Mouse.IsButtonReleased(button);
    public bool IsButtonUp(MouseButton button) => button == Button ? false : Mouse.IsButtonUp(button);
    public Vector2 Position { get { return Mouse.Position; } set { Mouse.Position = value; } }
    public Vector2 GetDelta() => Mouse.GetDelta();
    public Vector2 GetMouseWheelMove() => Mouse.GetMouseWheelMove();
}

public class MousePositionTransform : IMouse
{
    Func<Vector2, Vector2> Transform;
    Func<Vector2, Vector2> InverseTransform;
    private IMouse Mouse;
    public MousePositionTransform(IMouse mouse, Func<Vector2, Vector2> transform, Func<Vector2, Vector2> inverseTransform)
    {
        Mouse = mouse;
        Transform = transform;
        InverseTransform = inverseTransform;
    }
    public bool IsButtonPressed(MouseButton button) => Mouse.IsButtonPressed(button);
    public bool IsButtonDown(MouseButton button) => Mouse.IsButtonDown(button);
    public bool IsButtonReleased(MouseButton button) => Mouse.IsButtonReleased(button);
    public bool IsButtonUp(MouseButton button) => Mouse.IsButtonUp(button);
    public Vector2 Position
    {
        get { return Transform(Mouse.Position); }
        set { Mouse.Position = InverseTransform(value); }
    }
    public Vector2 GetDelta() => Mouse.GetDelta();
    public Vector2 GetMouseWheelMove() => Mouse.GetMouseWheelMove();
}