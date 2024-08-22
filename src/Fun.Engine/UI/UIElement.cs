using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.UI
{
    // TODO: it must be removed
    public abstract class UIElement
    {
        public Vector2 Position { get; set; }
        public bool IsVisible { get; set; }

        protected UIElement(Vector2 position)
        {
            Position = position;
            IsVisible = true;
        }

        public abstract void Draw(SpriteBatch spriteBatch);//Sprites sprites
        public abstract void Update(GameTime gameTime);
    }
}