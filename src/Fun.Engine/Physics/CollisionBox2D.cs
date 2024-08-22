using Microsoft.Xna.Framework;

namespace Fun.Engine.Physics
{
    public sealed class CollisionBox2D
    {
        private Vector2 _position = Vector2.Zero;

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public float PositionX
        {
            set => _position.X = value;
        }

        public float PositionY
        {
            set => _position.Y = value;
        }

        public float Width { get; set; }

        public float Height { get; set; }

        public Rectangle Rectangle => new((int)Position.X, (int)Position.Y, (int)Width, (int)Height);

        public CollisionBox2D(Vector2 position, float width, float height)
        {
            Position = position;
            Width = width;
            Height = height;
        }

        public bool CollidesWith(float thisSPX, CollisionBox2D other, float otherSPX)
        {
            if (thisSPX + Position.X < otherSPX + other.Position.X + other.Width &&
                thisSPX + Position.X + Width > otherSPX + other.Position.X &&
                Position.Y < other.Position.Y + other.Height &&
                Position.Y + Height > other.Position.Y)
            {
                return true;
            }

            return false;
        }
    }
}