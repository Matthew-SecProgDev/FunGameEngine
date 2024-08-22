using Fun.Engine.Entities.Interfaces;
using Fun.Engine.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.Entities
{
    public abstract class BaseObject : IRenderable, INotifiable
    {
        private Vector2 _position = Vector2.Zero;

        public Texture2D Texture { get; protected set; }

        public virtual Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public virtual float PositionX
        {
            set => _position.X = value;
        }

        public virtual float PositionY
        {
            set => _position.Y = value;
        }

        public bool IsActive { get; private set; } = true;

        public int ZIndex { get; init; }

        /// <summary>
        /// Renders an object within a game state and displays it on the screen.
        /// </summary>
        /// <param name="sprites"></param>
        public virtual void Render(Graphics.Sprites sprites)
        {
        }

        /// <summary>
        /// Performs operations on an object based on the game state's events.
        /// </summary>
        /// <param name="event"></param>
        public virtual void OnNotify(BaseGameStateEvent @event)
        {
        }

        /// <summary>
        /// This function is called to make an object within the game state inactive.
        /// Once called, the object is hidden from the screen, and no further rendering operations are performed on it.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;

            // Additional code can be added here if needed, such as:
            // - Trigger a fade out animation
            // - Log the event
            // - Notify other components or systems of the change
        }

        /// <summary>
        /// This function makes an object within the game state active.
        /// Upon activation, the object appears on the screen, and rendering operations are resumed on it.
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Invoked immediately after the object is created to perform any necessary setup.
        /// This function is called automatically and should not be called manually in your code.
        /// </summary>
        public virtual void Initialize()
        {
        }
    }
}