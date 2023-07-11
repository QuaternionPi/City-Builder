using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public class Vector2JsonConverter : System.Text.Json.Serialization.JsonConverter<Vector2>
    {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();
            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            String propertyName1 = reader.GetString() ?? throw new JsonException();
            reader.Read();
            float value1 = reader.GetSingle();
            reader.Read();

            String propertyName2 = reader.GetString() ?? throw new JsonException();
            reader.Read();
            float value2 = reader.GetSingle();
            reader.Read();

            float x;
            float y;
            if (propertyName1 == "X" && propertyName2 == "Y")
            {
                x = value1;
                y = value2;
            }
            else if (propertyName1 == "Y" && propertyName2 == "X")
            {
                y = value1;
                x = value2;
            }
            else
            {
                throw new JsonException($"Names must be 'X' and 'Y'. Propery 1: {propertyName1}, Property 2: {propertyName2}");
            }
            return new(x, y);
        }
        public override void Write(Utf8JsonWriter writer, Vector2 vector, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("X", vector.X);
            writer.WriteNumber("Y", vector.Y);
            writer.WriteEndObject();
        }
    }
    public class TerrainJsonConverter : System.Text.Json.Serialization.JsonConverter<Terrain>
    {
        public override Terrain Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
                throw new JsonException();
            if (reader.TokenType == JsonTokenType.PropertyName)
                throw new JsonException();

            String value = reader.GetString() ?? throw new Exception();
            return value.ToTerrain();
        }
        public override void Write(Utf8JsonWriter writer, Terrain terrain, JsonSerializerOptions options)
        {
            writer.WriteStringValue(terrain.ToString());
        }
    }
}