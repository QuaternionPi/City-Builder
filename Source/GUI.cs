using System;
using System.ComponentModel.Design;
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
            if (clickable.IsMoused())
                clickable.OnLeftClick();
        }
        public static void OnRightClick(this IClickable clickable, object? sender, EventArgs eventArgs)
        {
            if (clickable.IsMoused())
                clickable.OnRightClick();
        }
    }
    public class GUIManager
    {
        public GUIManager(int layers)
        {
            GUILayers = new GUILayer[layers];
            for (int i = 0; i < layers; i++)
            {
                GUILayers[i] = new GUILayer();
            }
        }
        public int Layers { get { return GUILayers.Length; } }
        protected GUILayer[] GUILayers;
        protected bool SomethingClicked;
        public void Attach(object attachee, int layer)
        {
            if (layer < 0)
                throw new IndexOutOfRangeException("Layer Cannot be less than 0");
            if (layer >= Layers)
                throw new IndexOutOfRangeException($"Layer Cannot be greater than the highest layer (Layer {Layers})");
            GUILayers[layer].Attach(attachee);
            if (attachee is IClickable clickable)
            {
                clickable.LeftClick += StopClickPropagation;
                clickable.RightClick += StopClickPropagation;
            }
        }
        public void RenderCycle()
        {
            for (int i = 0; i < Layers; i++)
            {
                GUILayers[i].RenderCycle();
            }
        }
        public void UpdateCycle()
        {
            for (int i = 0; i < Layers; i++)
            {
                GUILayers[i].UpdateCycle();
            }
        }
        public void ClickCycle()
        {
            SomethingClicked = false;
            for (int i = Layers - 1; i >= 0 && SomethingClicked == false; i--)
            {
                GUILayers[i].ClickCycle();
            }
        }
        protected void StopClickPropagation(object? sender, EventArgs eventArgs) => StopClickPropagation();
        protected void StopClickPropagation()
        {
            SomethingClicked = true;
        }
        protected class GUILayer
        {
            protected event EventHandler? Render;
            protected event EventHandler? Update;
            protected event EventHandler? LeftClick;
            protected event EventHandler? RightClick;
            public void Attach(object attachee)
            {
                if (attachee is IRenderable renderable)
                    Render += renderable.Render;
                if (attachee is IUpdatable updatable)
                    Update += updatable.Update;
                if (attachee is IClickable clickable)
                {
                    LeftClick += clickable.OnLeftClick;
                    RightClick += clickable.OnRightClick;
                }
            }
            public void RenderCycle()
            {
                Render?.Invoke(this, EventArgs.Empty);
            }
            public void UpdateCycle()
            {
                Update?.Invoke(this, EventArgs.Empty);
            }
            public void ClickCycle()
            {
                if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_BUTTON_LEFT))
                    LeftClick?.Invoke(this, EventArgs.Empty);
                if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_BUTTON_RIGHT))
                    RightClick?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}