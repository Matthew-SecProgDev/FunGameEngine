using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.UI
{
    public abstract class UIElement
    {
        public Vector2 Position { get; set; }
        public bool IsVisible { get; set; }

        protected UIElement(Vector2 position)
        {
            Position = position;
            IsVisible = true;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
    }
}