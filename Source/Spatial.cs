using System;
using System.Numerics;
using System.Text.Json;
using CityBuilder.Numerics;
using Raylib_cs;

namespace CityBuilder
{
    public enum Orentation { Top = 1, Bottom, Left, Right };
    public interface IScale
    {
        public float Scale { get; }
    }
    public interface IRotate
    {
        public Angle Angle { get; }
    }
    public interface IPosition
    {
        public float X { get; }
        public float Y { get; }
        public Vector2 Position { get; }
    }
    public interface IRectangle : IPosition
    {
        public float Width { get; }
        public float Height { get; }
        public Vector2 Dimensions { get; }
    }
    public interface ICircle : IPosition
    {
        public float Radius { get; }
    }
    public interface ITriangle : IPosition
    {
        public Vector2 Point1 { get; }
        public Vector2 Point2 { get; }
        public Vector2 Point3 { get; }
    }
}