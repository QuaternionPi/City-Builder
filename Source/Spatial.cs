using System;
using System.Numerics;
using System.Text.Json;
using Raylib_cs;

namespace CityBuilder
{
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

}