using System;
using System.Diagnostics;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public interface ISpline
    {
        public float Thickness { get; set; }
        public float Length { get; }
        public Vector2 StartPosition { get; set; }
        public Vector2 EndPosition { get; set; }
    }
    public class LineSegment : ISpline
    {
        public LineSegment()
        {
            Thickness = 5;
        }
        public float Thickness { get; set; }
        public float Length { get { return (StartPosition - EndPosition).Length(); } }
        public Vector2 StartPosition { get; set; }
        public Vector2 EndPosition { get; set; }
        public void Render()
        {
            Raylib.DrawLineEx(StartPosition, EndPosition, Thickness, Color.BLACK);
        }
    }
    public class Line<Segment> where Segment : ISpline
    {
        public Line()
        {
            Segments = new List<Segment>();
        }
        protected List<Segment> Segments;
        public bool Connects(Segment segment)
        {
            if (Segments.Count == 0)
                return true;
            if (segment.StartPosition == Segments.Last().EndPosition)
                return true;
            return false;
        }
        public void AddLine(Segment segment)
        {
            if (Connects(segment) == false)
                throw new Exception("Lines don't connect");
            Segments.Add(segment);
        }
    }
}