using System;
using System.Diagnostics;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public class Spline : IRenderable
    {
        public Spline(IScreen screen, List<Vector2> controlPoints)
        {
            Screen = screen;
            Color color = Color.BLACK;
            int thickness = 2;
            Style = new Linear(this, color, thickness);
            ControlPoints = controlPoints;
        }
        protected IScreen Screen;
        public DrawStyle Style;
        public List<Vector2> ControlPoints { get; protected set; }
        public void Render()
        {
            Style.Render();
        }
        public abstract class DrawStyle : IRenderable
        {
            protected DrawStyle(Spline spline, Color color, float thickness)
            {
                Spline = spline;
                Color = color;
                Thickness = thickness;
            }
            protected Spline Spline;
            protected Color Color;
            protected float Thickness;
            public abstract void Render();
        }
        public class Linear : DrawStyle
        {
            public Linear(Spline spline, Color color, float thickness) : base(spline, color, thickness)
            {

            }
            public override void Render()
            {
                for (int i = 1; i < Spline.ControlPoints.Count; i++)
                {
                    Vector2 start = Spline.ControlPoints[i - 1];
                    Vector2 end = Spline.ControlPoints[i];
                    Raylib.DrawLine((int)start.X, (int)start.Y, (int)end.X, (int)end.Y, Color.BLACK);
                }
            }
        }
        public class Cardinal : DrawStyle
        {
            public Cardinal(Spline spline, Color color, float thickness, float tension) : base(spline, color, thickness)
            {
                Tension = tension;
            }
            protected float Tension;
            public override void Render()
            {
                List<Vector2> points = Spline.ControlPoints.ToList();

                Vector2 virtualStart = 2 * points[0] - points[1];
                points.Insert(0, virtualStart);

                Vector2 virtualEnd = 2 * points[points.Count - 1] - points[points.Count - 2];
                points.Add(virtualEnd);

                for (int i = 2; i < points.Count - 1; i++)
                {
                    Vector2 previous = points[i - 2];
                    Vector2 start = points[i - 1];
                    Vector2 end = points[i];
                    Vector2 next = points[i + 1];

                    Vector2 velocityStart = end - previous;
                    Vector2 velocityEnd = next - start;

                    Vector2 controlStart = start + velocityStart * Tension;
                    Vector2 controlEnd = end - (velocityEnd * Tension);

                    Raylib.DrawLineBezierCubic(start, end, controlStart, controlEnd, 5, Color.BLACK);
                }
            }
        }
    }
}