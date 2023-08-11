using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public interface ICameraControler2D
    {
        public Vector2 Direction { get; }
        public float Zoom { get; }
        public float Rotation { get; }
    }
    public class KeyboardCameraControler : ICameraControler2D
    {
        public KeyboardCameraControler(IKeyboard keyboard)
        {
            Keyboard = keyboard;
        }
        protected IKeyboard Keyboard { get; }
        public Vector2 Direction
        {
            get
            {
                int x = 0;
                int y = 0;
                if (Keyboard.IsKeyDown(KeyboardKey.KEY_W)) y -= 1;
                if (Keyboard.IsKeyDown(KeyboardKey.KEY_S)) y += 1;
                if (Keyboard.IsKeyDown(KeyboardKey.KEY_A)) x -= 1;
                if (Keyboard.IsKeyDown(KeyboardKey.KEY_D)) x += 1;

                Vector2 direction = new Vector2(x, y);
                if (direction == Vector2.Zero)
                    return direction;
                return Vector2.Normalize(direction);
            }
        }
        public float Zoom
        {
            get
            {
                int zoom = 0;
                if (Keyboard.IsKeyDown(KeyboardKey.KEY_UP)) zoom += 1;
                if (Keyboard.IsKeyDown(KeyboardKey.KEY_UP)) zoom -= 1;
                return zoom;
            }
        }
        public float Rotation
        {
            get
            {
                int rotation = 0;
                if (Keyboard.IsKeyDown(KeyboardKey.KEY_Q)) rotation += 1;
                if (Keyboard.IsKeyDown(KeyboardKey.KEY_E)) rotation -= 1;
                return rotation;
            }
        }
    }
}