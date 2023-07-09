using System;
using System.Numerics;
using System.Text.Json;
using Raylib_cs;

namespace CityBuilder
{
    public interface IRenderable
    {
        public void Render();
        public bool Shown { get; }
    }
    public static class RenderableExtensions
    {
        public static void Render(this IRenderable renderable, object? sender, EventArgs eventArgs)
        {
            if (renderable.Shown) renderable.Render();
        }
    }
    public interface IUpdatable
    {
        public void Update();
        public bool Active { get; }
    }
    public static class UpdatableExtensions
    {
        public static void Update(this IUpdatable updatable, object? sender, EventArgs eventArgs)
        {
            if (updatable.Active) updatable.Update();
        }
    }
    public interface IMousable : IRenderable
    {
        public bool IsMoused();
    }
    public interface IClickable : IMousable
    {
        public void OnLeftClick();
        public void OnRightClick();
        public event EventHandler LeftClick;
        public event EventHandler RightClick;
    }
    public static class ClickableExtensions
    {
        public static void OnLeftClick(this IClickable clickable, object? sender, EventArgs eventArgs)
        {
            clickable.OnLeftClick();
        }
        public static void OnRightClick(this IClickable clickable, object? sender, EventArgs eventArgs)
        {
            clickable.OnRightClick();
        }
    }
}