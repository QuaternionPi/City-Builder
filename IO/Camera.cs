using CityBuilder.Numerics;

namespace CityBuilder.IO;

public readonly struct Camera
{
    private static System.Numerics.Vector2 SystemVector(Vector2 vector) =>
        new System.Numerics.Vector2(vector.X, vector.Y);
    private static Vector2 CustomVector(System.Numerics.Vector2 vector) =>
        new Vector2(vector.X, vector.Y);

    public static implicit operator Raylib_cs.Camera2D(Camera camera) =>
        new Raylib_cs.Camera2D(SystemVector(camera.Offset),
                               SystemVector(camera.Target),
                               (float)camera.Rotation, camera.Zoom);
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
        return CustomVector(Raylib_cs.Raylib.GetScreenToWorld2D(SystemVector(position), this));
    }
    public Vector2 GetWorldToScreen2D(Vector2 position)
    {
        return CustomVector(Raylib_cs.Raylib.GetWorldToScreen2D(SystemVector(position), this));
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
                Vector2.Zero,
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
        if (Position.X < 0)
        {
            Position += Vector2.UnitX * Width;
            PositionActual += Vector2.UnitX * Width;
        }
        if (Position.X > Width)
        {
            Position -= Vector2.UnitX * Width;
            PositionActual -= Vector2.UnitX * Width;
        }
        if (Position.Y < 0)
        {
            Position += Vector2.UnitY * Height;
            PositionActual += Vector2.UnitY * Height;
        }
        if (Position.Y > Height)
        {
            Position -= Vector2.UnitY * Height;
            PositionActual -= Vector2.UnitY * Height;
        }
        PositionActual = Vector2.LerpClamped(PositionActual, Position, deltaTime * Speed);
        ZoomActual = float.Lerp(ZoomActual, Zoom, Math.Clamp(deltaTime * Speed, 0, 1));
    }
}