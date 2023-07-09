using System;
using System.ComponentModel.Design.Serialization;
using System.Numerics;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace CityBuilder
{
    public class Angle : CustomValueType<Angle, double>
    {
        private Angle(double value) : base(value % (2 * Math.PI)) { }
        public static implicit operator Angle(double value) { return new Angle(value); }
        public static implicit operator double(Angle custom) { return custom._value; }
    }
    public class Vector2JsonConverter : System.Text.Json.Serialization.JsonConverter<Vector2>
    {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();
            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            String PropertyName1 = reader.GetString() ?? throw new JsonException();
            reader.Read();
            float Value1 = reader.GetSingle();
            reader.Read();

            String PropertyName2 = reader.GetString() ?? throw new JsonException();
            reader.Read();
            float Value2 = reader.GetSingle();
            reader.Read();

            float x;
            float y;
            if (PropertyName1 == "X" && PropertyName2 == "Y")
            {
                x = Value1;
                y = Value2;
            }
            else if (PropertyName1 == "Y" && PropertyName2 == "X")
            {
                y = Value1;
                x = Value2;
            }
            else
            {
                throw new JsonException($"Names must be 'X' and 'Y'. Propery 1: {PropertyName1}, Property 2: {PropertyName2}");
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
    public class CustomValueType<TCustom, TValue> where TValue : notnull
    {
        protected readonly TValue _value;
        public CustomValueType(TValue value)
        {
            _value = value;
        }
        public override string? ToString()
        {
            return _value.ToString();
        }

        public static bool operator <(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return Comparer<TValue>.Default.Compare(a._value, b._value) < 0;
        }

        public static bool operator >(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return !(a < b);
        }

        public static bool operator <=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return (a < b) || (a == b);
        }

        public static bool operator >=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return (a > b) || (a == b);
        }

        public static bool operator ==(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return a.Equals((object)b);
        }

        public static bool operator !=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return !(a == b);
        }

        public static TCustom operator +(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return (dynamic)a._value + b._value;
        }

        public static TCustom operator -(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return ((dynamic)a._value - b._value);
        }

        protected bool Equals(CustomValueType<TCustom, TValue> other)
        {
            return EqualityComparer<TValue>.Default.Equals(_value, other._value);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CustomValueType<TCustom, TValue>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TValue>.Default.GetHashCode(_value);
        }
    }
}