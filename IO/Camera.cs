using CityBuilder.Numerics;

namespace CityBuilder.IO;

using SystemVector2 = System.Numerics.Vector2;

public readonly struct Camera
{
    public static implicit operator Raylib_cs.Camera2D(Camera camera) =>
        new Raylib_cs.Camera2D(
            (SystemVector2)camera.Offset,
            (SystemVector2)camera.Target,
            (float)camera.Rotation, camera.Zoom
        );
    public readonly Vector2 Offset { get; }
    public readonly Vector2 Target { get; }
    public readonly Degree Rotation { get; }
    public readonly float Zoom { get; }
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
    public Vector2 GetScreenToWorld2D(Vector2 position)
    {
        return (Vector2)Raylib_cs.Raylib.GetScreenToWorld2D((SystemVector2)position, this);
    }
    public Vector2 GetWorldToScreen2D(Vector2 position)
    {
        return (Vector2)Raylib_cs.Raylib.GetWorldToScreen2D((SystemVector2)position, this);
    }
}

public class CameraMount
{
    private Vector2 PositionActual { get; set; }
    public Vector2 Position { get; set; }
    public float Width { get; protected set; }
    public float Height { get; protected set; }
    private float ZoomActual { get; set; }
    public float Zoom { get; set; }
    public float Speed;
    public Camera Camera
    {
        get
        {
            return new Camera(
                Window.GetDimensions() / 2,
                PositionActual,
                0,
                ZoomActual
            );
        }
    }
    public CameraMount(
        Vector2 position,
        float width,
        float height,
        float zoom = 1,
        float speed = 1
    )
    {
        Position = position;
        PositionActual = position;
        Width = width;
        Height = height;
        Zoom = zoom;
        ZoomActual = zoom;
        Speed = speed;
    }
    public void Update(float deltaTime)
    {
        Zoom = Math.Clamp(Zoom, Math.Max(Window.GetDimensions().Y / Height, Window.GetDimensions().X / Width), 1000); // Ensure zoom stays positive

        PositionActual = Vector2.LerpClamped(PositionActual, Position, deltaTime * Speed);
        ZoomActual = float.Lerp(ZoomActual, Zoom, Math.Clamp(deltaTime * Speed, 0, 1));
        if (Position.X < Width * 0.5)
        {
            Position += Vector2.UnitX * Width;
            PositionActual += Vector2.UnitX * Width;
        }
        if (Position.X > Width * 1.5)
        {
            Position -= Vector2.UnitX * Width;
            PositionActual -= Vector2.UnitX * Width;
        }
        if (PositionActual.Y < Height * (2.5 / Zoom))
        {
            PositionActual = (Vector2.UnitY * Height * (2.5f / Zoom)) + (Vector2.UnitX * Position.X);
        }
        if (Position.Y < Height * (2.5 / Zoom))
        {
            Position = (Vector2.UnitY * Height * (2.5f / Zoom)) + (Vector2.UnitX * Position.X);
        }
        if (PositionActual.Y > Height * (-2.5 / Zoom + 1))
        {
            PositionActual = (Vector2.UnitY * Height * (-2.5f / Zoom + 1)) + (Vector2.UnitX * Position.X);
        }
        if (Position.Y > Height * (-2.5 / Zoom + 1))
        {
            Position = (Vector2.UnitY * Height * (-2.5f / Zoom + 1)) + (Vector2.UnitX * Position.X);
        }
    }
}