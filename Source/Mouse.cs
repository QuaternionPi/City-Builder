using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public interface IMouse
    {
        public Vector2 Position { get; }
        public delegate void Del(bool mouseBlocked);
        public delegate void MousePress(IMouse sender, MouseButton button);
        public delegate void MouseRelease(IMouse sender, MouseButton button);
        public event Del? UpdateIsMoused;
        public event MousePress? MousePressed;
        public event MouseRelease? MouseReleased;
        public void Block();
        public bool IsAnythingMoused(bool mouseBlocked);
        public void CheckForClick();
    }
    public class RaylibMouse : IMouse
    {
        public Vector2 Position { get { return Raylib.GetMousePosition(); } }
        public event IMouse.Del? UpdateIsMoused;
        public event IMouse.MousePress? MousePressed;
        public event IMouse.MouseRelease? MouseReleased;
        protected bool Blocked;
        public void Block()
        {
            Blocked = true;
        }
        public bool IsAnythingMoused(bool mouseBlocked)
        {
            Blocked = false;
            UpdateIsMoused?.Invoke(mouseBlocked);
            return Blocked;
        }
        public void CheckForClick()
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                MousePressed?.Invoke(this, MouseButton.MOUSE_LEFT_BUTTON);
            }
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_RIGHT_BUTTON))
            {
                MousePressed?.Invoke(this, MouseButton.MOUSE_RIGHT_BUTTON);
            }
            if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
            {
                MouseReleased?.Invoke(this, MouseButton.MOUSE_LEFT_BUTTON);
            }
            if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_RIGHT_BUTTON))
            {
                MouseReleased?.Invoke(this, MouseButton.MOUSE_RIGHT_BUTTON);
            }
        }
    }
    public class CameraOffsetMouse : IMouse
    {
        public CameraOffsetMouse(IMouse mouse)
        {
            Mouse = mouse;
        }
        protected IMouse Mouse;
        public void UpdatePosition(Camera2D camera)
        {
            Position = Mouse.Position + camera.target;
        }
        public Vector2 Position { get; protected set; }
        public event IMouse.Del? UpdateIsMoused;
        public event IMouse.MousePress? MousePressed;
        public event IMouse.MouseRelease? MouseReleased;
        protected bool Blocked;
        public void Block()
        {
            Blocked = true;
        }
        public bool IsAnythingMoused(bool mouseBlocked)
        {
            Blocked = false;
            UpdateIsMoused?.Invoke(mouseBlocked);
            return Blocked;
        }
        public void CheckForClick()
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                MousePressed?.Invoke(this, MouseButton.MOUSE_LEFT_BUTTON);
            }
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_RIGHT_BUTTON))
            {
                MousePressed?.Invoke(this, MouseButton.MOUSE_RIGHT_BUTTON);
            }
            if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
            {
                MouseReleased?.Invoke(this, MouseButton.MOUSE_LEFT_BUTTON);
            }
            if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_RIGHT_BUTTON))
            {
                MouseReleased?.Invoke(this, MouseButton.MOUSE_RIGHT_BUTTON);
            }
        }
    }
}
