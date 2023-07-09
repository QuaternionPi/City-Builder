using System;
using System.Numerics;
using System.Text.Json;
using Raylib_cs;

namespace CityBuilder
{
    public interface IKinematic : IPosition
    {
        public Vector2 Velocity { get; }
        public Vector2 Acceleration { get; }
    }
    public interface IRotationKinematic : IPosition, IRotate
    {
        public Angle AngularVelocity { get; }
        public Angle AngularAcceleration { get; }
    }
    public static class KinematicExtensions
    {
        public static Vector2 ChangeInPosition(this IKinematic kinematic)
        {
            return kinematic.Velocity * Raylib.GetFrameTime();
        }
        public static Vector2 ChangeInVelocity(this IKinematic kinematic)
        {
            return kinematic.Acceleration * Raylib.GetFrameTime();
        }
        public static Angle ChangeInAngle(this IRotationKinematic kinematic)
        {
            return kinematic.AngularVelocity * Raylib.GetFrameTime();
        }
        public static Angle ChangeInAngularVelocity(this IRotationKinematic kinematic)
        {
            return kinematic.AngularAcceleration * Raylib.GetFrameTime();
        }
    }
}