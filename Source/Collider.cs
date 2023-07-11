using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    [JsonDerivedType(typeof(RectangleCollider), typeDiscriminator: "Rectangle")]
    [JsonDerivedType(typeof(CircleCollider), typeDiscriminator: "Circle")]
    [JsonDerivedType(typeof(TriangleCollider), typeDiscriminator: "Triangle")]
    public interface ICollider
    {
        public bool Collides(Vector2 point);
        public bool Collides(ICollider collider);
    }
    public struct RectangleCollider : ICollider, IRectangle
    {
        public RectangleCollider(Vector2 position, Vector2 dimensions)
        {
            Position = position;
            Dimensions = dimensions;
        }
        public RectangleCollider(Rectangle rectangle)
        {
            Rectangle = rectangle;
        }
        public float X { readonly get { return Position.X; } private set { Position = new(value, Position.Y); } }
        public float Y { readonly get { return Position.Y; } private set { Position = new(Position.X, value); } }
        [JsonInclude]
        [JsonConverter(typeof(Vector2JsonConverter))]
        public Vector2 Position { readonly get; private set; }
        public float Width { readonly get { return Dimensions.X; } private set { Dimensions = new(value, Dimensions.Y); } }
        public float Height { readonly get { return Dimensions.Y; } private set { Dimensions = new(Dimensions.X, value); } }
        [JsonInclude]
        [JsonConverter(typeof(Vector2JsonConverter))]
        public Vector2 Dimensions { readonly get; private set; }
        public Rectangle Rectangle
        {
            readonly get { return new(X - Width / 2, Y - Height / 2, Width, Height); }
            private set { X = value.x; Y = value.y; Width = value.width; Height = value.height; }
        }
        public readonly bool Collides(Vector2 point)
        {
            return Raylib.CheckCollisionPointRec(point, Rectangle);
        }
        public readonly bool Collides(ICollider collider) => collider switch
        {
            RectangleCollider => ColliderExtensions.Collides((RectangleCollider)collider, this),
            CircleCollider => ColliderExtensions.Collides((CircleCollider)collider, this),
            TriangleCollider => ColliderExtensions.Collides((TriangleCollider)collider, this),
            _ => throw new Exception($"Cannot check collision between {this} and {collider}"),
        };
    }
    public struct CircleCollider : ICollider, ICircle
    {
        public float X { readonly get { return Position.X; } private set { Position = new(value, Position.Y); } }
        public float Y { readonly get { return Position.Y; } private set { Position = new(Position.X, value); } }
        [JsonInclude]
        [JsonConverter(typeof(Vector2JsonConverter))]
        public Vector2 Position { readonly get; private set; }
        [JsonInclude]
        public float Radius { readonly get; private set; }
        public readonly bool Collides(Vector2 point)
        {
            return Raylib.CheckCollisionPointCircle(point, Position, Radius);
        }
        public readonly bool Collides(ICollider collider) => collider switch
        {
            RectangleCollider => ColliderExtensions.Collides((RectangleCollider)collider, this),
            CircleCollider => ColliderExtensions.Collides((CircleCollider)collider, this),
            TriangleCollider => ColliderExtensions.Collides((TriangleCollider)collider, this),
            _ => throw new Exception($"Cannot check collision between {this} and {collider}"),
        };
    }
    public struct TriangleCollider : ICollider, ITriangle
    {
        public TriangleCollider(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
        }
        public float X { readonly get { return Position.X; } private set { Position = new(value, Position.Y); } }
        public float Y { readonly get { return Position.Y; } private set { Position = new(Position.X, value); } }
        public Vector2 Position
        {
            readonly get { return (Point1 + Point2 + Point3) / 3; }
            private set
            {
                Vector2 delta = value - Position;
                Point1 += delta;
                Point2 += delta;
                Point3 += delta;
            }
        }
        [JsonInclude]
        [JsonConverter(typeof(Vector2JsonConverter))]
        public Vector2 Point1 { get; private set; }
        [JsonInclude]
        [JsonConverter(typeof(Vector2JsonConverter))]
        public Vector2 Point2 { get; private set; }
        [JsonInclude]
        [JsonConverter(typeof(Vector2JsonConverter))]
        public Vector2 Point3 { get; private set; }
        public readonly bool Collides(Vector2 point)
        {
            return Raylib.CheckCollisionPointTriangle(point, Point1, Point2, Point3);
        }
        public readonly bool Collides(ICollider collider) => collider switch
        {
            RectangleCollider => ColliderExtensions.Collides((RectangleCollider)collider, this),
            CircleCollider => ColliderExtensions.Collides((CircleCollider)collider, this),
            TriangleCollider => ColliderExtensions.Collides((TriangleCollider)collider, this),
            _ => throw new Exception($"Cannot check collision between {this} and {collider}"),
        };
    }
    public static class ColliderExtensions
    {
        public static bool Collides(RectangleCollider collider1, RectangleCollider collider2)
        {
            return Raylib.CheckCollisionRecs(collider1.Rectangle, collider2.Rectangle);
        }
        public static bool Collides(CircleCollider collider1, CircleCollider collider2)
        {
            return Raylib.CheckCollisionCircles(collider1.Position, collider1.Radius, collider2.Position, collider2.Radius);
        }
        public static bool Collides(TriangleCollider collider1, TriangleCollider collider2)
        {
            bool anyCollision = false;
            anyCollision |= Raylib.CheckCollisionPointTriangle(collider1.Point1, collider2.Point1, collider2.Point2, collider2.Point3);
            anyCollision |= Raylib.CheckCollisionPointTriangle(collider1.Point2, collider2.Point1, collider2.Point2, collider2.Point3);
            anyCollision |= Raylib.CheckCollisionPointTriangle(collider1.Point3, collider2.Point1, collider2.Point2, collider2.Point3);
            return anyCollision;
        }
        public static bool Collides(CircleCollider collider1, RectangleCollider collider2) => Collides(collider2, collider1);
        public static bool Collides(RectangleCollider collider1, CircleCollider collider2)
        {
            return Raylib.CheckCollisionCircleRec(collider2.Position, collider2.Radius, collider1.Rectangle);
        }
        public static bool Collides(TriangleCollider collider1, RectangleCollider collider2) => Collides(collider2, collider1);
        public static bool Collides(RectangleCollider collider1, TriangleCollider collider2)
        {
            bool anyCollision = false;
            anyCollision |= Raylib.CheckCollisionPointRec(collider2.Point1, collider1.Rectangle);
            anyCollision |= Raylib.CheckCollisionPointRec(collider2.Point2, collider1.Rectangle);
            anyCollision |= Raylib.CheckCollisionPointRec(collider2.Point3, collider1.Rectangle);
            return anyCollision;
        }
        public static bool Collides(TriangleCollider collider1, CircleCollider collider2) => Collides(collider2, collider1);
        public static bool Collides(CircleCollider collider1, TriangleCollider collider2)
        {
            throw new NotImplementedException();
        }
    }
}