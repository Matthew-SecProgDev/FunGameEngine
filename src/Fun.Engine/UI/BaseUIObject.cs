using Fun.Engine.Entities.Interfaces;
using Microsoft.Xna.Framework;

namespace Fun.Engine.UI
{
    /// <summary>
    /// This class serves as the base class for UI elements in the game,
    /// providing foundational properties and methods.
    /// </summary>
    public abstract class BaseUIObject : Entities.BaseObject, IUpdatable
    {
        private Vector2 _position = Vector2.Zero;

        public Vector2 Size { get; }

        public new Vector2 Position
        {
            //check it
            get => _position;
            set => _position = value;
        }

        public new float PositionX
        {
            //check it
            set => _position.X = value;
        }

        public new float PositionY
        {
            //check it
            set => _position.Y = value;
        }

        public BaseUIObject(Vector2 size, Vector2 position)
        {
            Size = size;
            Position = position;
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}