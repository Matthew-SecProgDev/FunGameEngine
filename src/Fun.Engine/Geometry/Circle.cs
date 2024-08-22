using Microsoft.Xna.Framework;

namespace Fun.Engine.Geometry
{
    public readonly struct Circle
    {
        public Vector2 Center { get; }

        public float Radius { get; }

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public Circle(float x, float y, float radius)
        {
            Center = new Vector2(x, y);
            Radius = radius;
        }
    }
}