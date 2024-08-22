using Fun.Engine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.Physics
{
    /// <summary>
    /// This base class is designed to be inherited by various game objects that need to check for collisions with other objects.
    /// </summary>
    public abstract class BaseGameCollidableObject : BaseGameObject
    {
        private readonly List<CollisionBox2D> _collisionBoxes = new(3);

        private Texture2D? _cbTexture;//CollisionBox2D

        public BaseGameCollidableObject? Owner { get; init; }

        public IEnumerable<CollisionBox2D> CollisionBoxes => _collisionBoxes;

        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                var deltaX = value.X - base.Position.X;
                var deltaY = value.Y - base.Position.Y;

                base.PositionX = value.X;
                base.PositionY = value.Y;

                foreach (var collisionBox2D in _collisionBoxes)
                {
                    collisionBox2D.PositionX = collisionBox2D.Position.X + deltaX;
                    collisionBox2D.PositionY = collisionBox2D.Position.Y + deltaY;
                }
            }
        }

        public override float PositionX
        {
            set
            {
                var deltaX = value - base.Position.X;

                base.PositionX = value;

                foreach (var collisionBox2D in _collisionBoxes)
                {
                    collisionBox2D.PositionX = collisionBox2D.Position.X + deltaX;
                }
            }
        }

        public override float PositionY
        {
            set
            {
                var deltaY = value - base.Position.Y;

                base.PositionY = value;

                foreach (var collisionBox2D in _collisionBoxes)
                {
                    collisionBox2D.PositionY = collisionBox2D.Position.Y + deltaY;
                }
            }
        }

        /// <summary>
        /// Adds a CollisionBox2D to the game object’s private list for collision detection.
        /// </summary>
        /// <param name="collisionBox2D"></param>
        public void AddCollisionBox(CollisionBox2D collisionBox2D)
        {
            _collisionBoxes.Add(collisionBox2D);
        }

        /// <summary>
        /// Draws all CollisionBox2D instances of the object on the screen,
        /// useful for debugging the placement of collision boxes.
        /// </summary>
        /// <param name="sprites"></param>
        public void RenderCollisionBox(Graphics.Sprites sprites)
        {
            if (Destroyed)
            {
                return;
            }

            if (_cbTexture == null)
            {
                _cbTexture = new Texture2D(sprites.GraphicsDevice, 1, 1);
                _cbTexture.SetData([Color.White]);
            }

            foreach (var collisionBox2D in _collisionBoxes)
            {
                sprites.Draw(_cbTexture,
                    collisionBox2D.Position + ScreenPosition,
                    collisionBox2D.Rectangle,
                    Color.Red);
            }
        }

        public override void Destroy()
        {
            base.Destroy();

            _cbTexture?.Dispose();
        }
    }
}