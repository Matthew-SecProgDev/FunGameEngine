using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.Engine.UI
{
    public class ProgressBar : UIElement
    {
        private Texture2D backgroundTexture;
        private Texture2D fillTexture;
        private float currentValue;
        private float maxValue;
        private Rectangle backgroundRectangle;
        private Rectangle fillRectangle;

        public float CurrentValue
        {
            get => currentValue;
            set => currentValue = FunMath.Clamp(value, 0, maxValue);
        }

        public float MaxValue
        {
            get => maxValue;
            set => maxValue = value;
        }

        public ProgressBar(Vector2 position, Texture2D background, Texture2D fill, float maxValue)
            : base(position)
        {
            this.backgroundTexture = background;
            this.fillTexture = fill;
            this.maxValue = maxValue;
            this.currentValue = 0;

            this.backgroundRectangle = new Rectangle((int)position.X, (int)position.Y, background.Width, background.Height);
            this.fillRectangle = new Rectangle(backgroundRectangle.X, backgroundRectangle.Y, 0, background.Height);
        }

        public override void Update(GameTime gameTime)
        {
            int width = (int)(currentValue / maxValue * backgroundRectangle.Width);
            fillRectangle.Width = width;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, backgroundRectangle, Color.White);
            spriteBatch.Draw(fillTexture, fillRectangle, Color.White);
        }
    }
}