using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public interface IKeyboard
    {
        public bool IsKeyDown(KeyboardKey key);
    }
    public class RaylibKeyboard : IKeyboard
    {
        public bool IsKeyDown(KeyboardKey key) => Raylib.IsKeyDown(key);
    }
}