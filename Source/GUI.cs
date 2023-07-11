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
        public event EventHandler? LeftClicked;
        public event EventHandler? RightClicked;
        public event EventHandler? LeftDragged;
        public event EventHandler? RightDragged;
        public void LeftClick();
        public void RightClick();
        public void LeftDrag();
        public void RightDrag();
    }
    public static class ClickableExtensions
    {
        public static void LeftClick(this IClickable clickable, object? sender, EventArgs eventArgs)
        {
            if (clickable.IsMoused())
                clickable.LeftClick();
        }
        public static void RightClick(this IClickable clickable, object? sender, EventArgs eventArgs)
        {
            if (clickable.IsMoused())
                clickable.RightClick();
        }
        public static void LeftDrag(this IClickable clickable, object? sender, EventArgs eventArgs)
        {
            if (clickable.IsMoused())
                clickable.LeftDrag();
        }
        public static void RightDrag(this IClickable clickable, object? sender, EventArgs eventArgs)
        {
            if (clickable.IsMoused())
                clickable.RightDrag();
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
                clickable.LeftClicked += StopClickPropagation;
                clickable.RightClicked += StopClickPropagation;
                clickable.LeftDragged += StopClickPropagation;
                clickable.RightDragged += StopClickPropagation;
            }
        }
        public void RenderCycle()
        {
            for (int i = 0; i < Layers; i++)
            {
                GUILayers[i].Render();
            }
        }
        public void UpdateCycle()
        {
            for (int i = 0; i < Layers; i++)
            {
                GUILayers[i].Update();
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
        protected class GUILayer : IRenderable, IUpdatable, IClickable
        {
            public GUILayer()
            {
                Shown = true;
                Active = true;
            }
            public bool Shown { get; }
            public bool Active { get; }
            public event EventHandler? LeftClicked;
            public event EventHandler? RightClicked;
            public event EventHandler? LeftDragged;
            public event EventHandler? RightDragged;
            protected event EventHandler? RenderEvent;
            protected event EventHandler? UpdateEvent;
            protected event EventHandler? LeftClickEvent;
            protected event EventHandler? RightClickEvent;
            protected event EventHandler? LeftDragEvent;
            protected event EventHandler? RightDragEvent;
            public void Attach(object attachee)
            {
                if (attachee is IRenderable renderable)
                    RenderEvent += renderable.Render;
                if (attachee is IUpdatable updatable)
                    UpdateEvent += updatable.Update;
                if (attachee is IClickable clickable)
                {
                    LeftClickEvent += clickable.LeftClick;
                    RightClickEvent += clickable.RightClick;
                    LeftDragEvent += clickable.LeftDrag;
                    RightDragEvent += clickable.RightDrag;
                }
            }
            public void Render()
            {
                RenderEvent?.Invoke(this, EventArgs.Empty);
            }
            public void Update()
            {
                UpdateEvent?.Invoke(this, EventArgs.Empty);
            }
            public bool IsMoused() => true;
            public void LeftClick()
            {
                LeftClickEvent?.Invoke(this, EventArgs.Empty);
            }
            public void RightClick()
            {
                RightClickEvent?.Invoke(this, EventArgs.Empty);
            }
            public void LeftDrag()
            {
                LeftDragEvent?.Invoke(this, EventArgs.Empty);
            }
            public void RightDrag()
            {
                RightDragEvent?.Invoke(this, EventArgs.Empty);
            }
            public void ClickCycle()
            {
                if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_BUTTON_LEFT))
                    LeftClick();
                if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_BUTTON_RIGHT))
                    RightClick();
                if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
                    LeftDrag();
                if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT))
                    RightDrag();
            }
        }
    }
}