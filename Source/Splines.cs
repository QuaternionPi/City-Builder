using System;
using System.Diagnostics;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public class LineSegment : IRenderable
    {
        public LineSegment()
        {
            Shown = true;
        }
        public bool Shown { get; }
        public Vector2 StartPosition;
        public Vector2 EndPosition;
        public void Render()
        {
            Raylib.DrawLineEx(StartPosition, EndPosition, 5, Color.BLACK);
        }
        public void Initialize(GUIManager guiManager)
        {
            guiManager.Attach(this, 3);
        }
    }
}