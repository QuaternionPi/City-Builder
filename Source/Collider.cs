using System;
using System.Numerics;
using System.Text.Json;
using Raylib_cs;

namespace CityBuilder
{
    public interface ICollider
    {
        public bool Collides(Vector2 point);
        public bool Collides(ICollider collider);
    }
}