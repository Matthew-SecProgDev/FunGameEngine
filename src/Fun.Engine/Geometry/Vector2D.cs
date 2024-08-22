using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.Engine.Geometry
{
    public readonly struct Vector2D : IEquatable<Vector2D>
    {
        public readonly float X;
        public readonly float Y;

        public static readonly Vector2D Zero = new(0f, 0f);
        public static readonly Vector2D UnitX = new(1f, 0f);
        public static readonly Vector2D UnitY = new(0f, 1f);

        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2D operator -(Vector2D vector)
        {
            return new Vector2D(-vector.X, -vector.Y);
        }

        public static Vector2D operator *(Vector2D vector, float scalar)
        {
            return new Vector2D(vector.X * scalar, vector.Y * scalar);
        }

        public static Vector2D operator *(float scalar, Vector2D vector)
        {
            return new Vector2D(vector.X * scalar, vector.Y * scalar);
        }

        public static Vector2D operator /(Vector2D vector, float scalar)
        {
            return new Vector2D(vector.X / scalar, vector.Y / scalar);
        }

        public static Vector2D operator /(float scalar, Vector2D vector)
        {
            return new Vector2D(vector.X / scalar, vector.Y / scalar);
        }

        public static bool operator ==(Vector2D a, Vector2D b) => a.Equals(b);

        public static bool operator !=(Vector2D a, Vector2D b) => !(a == b);

        public override bool Equals(object? obj) => obj is Vector2D other && Equals(other);

        public bool Equals(Vector2D other)
        {
            return FunMath.IsNearlyEqual(X, other.X) && FunMath.IsNearlyEqual(Y, other.Y);
        }

        public override int GetHashCode() => new { X, Y }.GetHashCode();

        public override string ToString() => $"X: {X}, Y: {Y}";
    }
}