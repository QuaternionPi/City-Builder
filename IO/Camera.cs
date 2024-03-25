using CityBuilder.Numerics;

namespace CityBuilder.IO;

public readonly struct Camera
{
    private static System.Numerics.Vector2 SystemVector(Vector2 vector) => new System.Numerics.Vector2(vector.X, vector.Y);
    public static implicit operator Raylib_cs.Camera2D(Camera camera) => new Raylib_cs.Camera2D(SystemVector(camera.Offset), SystemVector(camera.Target), (float)camera.Rotation, camera.Zoom);
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
}

public class CameraMount
{
    public Camera Camera
    {
        get
        {
            return new Camera(
                Vector2.Zero,
                PositionActual,
                0,
                1
            );
        }
    }
    public float Speed;
    private Vector2 PositionActual { get; set; }
    public Vector2 Position { get; set; }
    public CameraMount(Vector2 position, float speed = 1)
    {
        Position = position;
        PositionActual = position;
        Speed = speed;
    }
    public void Update(float deltaTime) =>
        PositionActual = Vector2.LerpClamped(PositionActual, Position, deltaTime * Speed);
}