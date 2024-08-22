using Fun.Engine.Geometry;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

namespace Fun.Engine.Utilities
{
    public static class MathHelper
    {
        public const float Epsilon = 1e-6f;

        public static int Clamp(int value, int min, int max)
        {
            if (min > max)
            {
                const string msg = "The value of 'min' is greater than the value of 'max'.";
                throw new ArgumentOutOfRangeException(null, msg);
            }

            if (value < min)
            {
                return min;
            }

            return value > max ? max : value;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (min > max)
            {
                const string msg = "The value of 'min' is greater than the value of 'max'.";
                throw new ArgumentOutOfRangeException(null, msg);
            }

            if (value < min)
            {
                return min;
            }

            return value > max ? max : value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(ref float x, ref float y)
        {
            var invertedLen = 1f / MathF.Sqrt(x * x + y * y);
            x *= invertedLen;
            y *= invertedLen;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector2 a, Vector2 b)
        {
            var deltaX = a.X - b.X;
            var deltaY = a.Y - b.Y;
            return MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSquared(Vector2 a, Vector2 b)
        {
            var deltaX = a.X - b.X;
            var deltaY = a.Y - b.Y;
            return deltaX * deltaX + deltaY * deltaY;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LengthSquared(Vector2 a)
        {
            return a.X * a.X + a.Y * a.Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DotProduct(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CrossProduct(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static T GetItem<T>(T[] array, int index)
        {
            if (index >= array.Length)
            {
                return array[index % array.Length];
            }

            if (index < 0)
            {
                return array[index % array.Length + array.Length];
            }

            return array[index];
        }

        public static T GetItem<T>(List<T> list, int index)
        {
            if (index >= list.Count)
            {
                return list[index % list.Count];
            }

            if (index < 0)
            {
                return list[index % list.Count + list.Count];
            }

            return list[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNearlyEqual(float a, float b)
        {
            return MathF.Abs(a - b) < Epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNearlyZero(float value)
        {
            return MathF.Abs(value) < Epsilon;
        }

        public static Vector2 Transform(Vector2 position, Transform2D transform)
        {
            return new Vector2(
                position.X * transform.CosScaleX - position.Y * transform.SinScaleY + transform.PosX,
                position.X * transform.SinScaleX + position.Y * transform.CosScaleY + transform.PosY);
        }
    }
}