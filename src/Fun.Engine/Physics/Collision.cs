using Fun.Engine.Geometry;
using Microsoft.Xna.Framework;
using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.Engine.Physics
{
    public static class Collision
    {
        public static bool IntersectCircle(Circle a, Circle b)
        {
            var distanceSquared = FunMath.DistanceSquared(a.Center, b.Center);
            var combinedRadii = a.Radius + b.Radius;
            var combinedRadiiSquared = combinedRadii * combinedRadii;

            return distanceSquared < combinedRadiiSquared;
        }

        public static bool IntersectCircle(Circle a, Circle b, out float depth, out Vector2 normal)
        {
            depth = 0f;
            normal = Vector2.Zero;

            var difference = b.Center - a.Center;
            //var distanceSquared = difference.LengthSquared();
            var distanceSquared = FunMath.LengthSquared(difference);
            var combinedRadii = a.Radius + b.Radius;
            var combinedRadiiSquared = combinedRadii * combinedRadii;

            if (distanceSquared >= combinedRadiiSquared)
            {
                return false;
            }

            var distance = MathF.Sqrt(distanceSquared);

            if (distance != 0f)
            {
                depth = combinedRadii - distance;
                normal = difference / distance;
            }
            else
            {
                depth = combinedRadii;
                normal = Vector2.UnitX;
            }

            return true;
        }
    }
}