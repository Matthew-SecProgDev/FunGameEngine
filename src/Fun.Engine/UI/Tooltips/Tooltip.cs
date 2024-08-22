using Fun.Engine.Graphics;
using Fun.Engine.UI.Tooltips.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.UI.Tooltips
{
    // TODO: it must be refactored and inherited from BaseUIObject


    //public class PersistentTooltip : SimpleTooltip
    //{
    //    private bool isVisible;

    //    public PersistentTooltip(Vector2 size, TooltipOption option, Vector2 position) : base(size, option, position)
    //    {
    //    }
    //}

    public class SimpleTooltip : BaseTooltip
    {
        //    // text should be middle of bound
        //    // I should determine a position that every time it is hovered by mouse, then display tooltip and
        //    // when mouse is exited of bound, the tooltip is faded away gradually
        public SimpleTooltip(Vector2 size, TooltipOption option, Vector2 position) : base(size, option, position)
        {
        }
    }

    public abstract class BaseTooltip : BaseUIObject
    {
        private readonly Texture2D _backgroundTexture;
        private readonly Color _penColor;
        private Rectangle _tooltipBounds;
        private Vector2 _textPosition;

        public Rectangle TooltipBounds
        {
            get => _tooltipBounds; 
            protected set => _tooltipBounds = value;    //maybe this property should be private, maybe not and init
        }

        public string Text { get; private set; }    //maybe this property should be protected, maybe not

        public Vector2 TextPosition
        {
            get => _textPosition; 
            protected set => _textPosition = value;     //maybe this property should be private, maybe not and init
        }

        protected SpriteFont Font { get; }

        protected BaseTooltip(Vector2 size, TooltipOption option, Vector2 position) : base(size, position)
        {
            _backgroundTexture = option.BackgroundTexture;
            _penColor = option.PenColor;

            Text = option.Text;
            Font = option.Font;

            var tooltipPosition = new Point((int)Position.X, (int)Position.Y);
            var tooltipSize = new Point((int)Size.X, (int)Size.Y);
            TooltipBounds = new Rectangle(tooltipPosition, tooltipSize);

            TextPosition = new Vector2(tooltipPosition.X + 5, tooltipPosition.Y + 5);
        }

        public override void Render(Sprites sprites)
        {
            sprites.Draw(_backgroundTexture, TooltipBounds, null, Color.White);
            sprites.DrawString(Font, Text, TextPosition, _penColor);
        }

        public void UpdateText(string text)
        {
            Text = text;
        }

        public void UpdateTextPosition(int x, int y)
        {
            _textPosition.X = x - Font.MeasureString(Text).Y / 2;
            _textPosition.Y = y - Font.MeasureString(Text).Y / 2;
        }

        public void UpdateTooltipPosition(int x, int y)
        {
            _tooltipBounds.X = x;
            _tooltipBounds.Y = y;
        }

        public void UpdateTooltipSize(int width, int height)
        {
            _tooltipBounds.Width = width;
            _tooltipBounds.Height = height;
        }
    }

    public class Tooltip2 : UIElement
    {
        private string text;
        private SpriteFont font;
        private Texture2D backgroundTexture;
        private Vector2 textPosition;
        private Rectangle backgroundRectangle;
        private bool isVisible;
        private double hoverTime;
        private double tooltipDelay = 1.0;  // Delay in seconds before showing the tooltip

        public Tooltip2(string text, SpriteFont font, Texture2D background, Vector2 position)
            : base(position)
        {
            this.text = text;
            this.font = font;
            backgroundTexture = background;
            isVisible = false;
            hoverTime = 0;

            Vector2 textSize = font.MeasureString(text);
            backgroundRectangle = new Rectangle((int)position.X, (int)position.Y, (int)textSize.X + 10, (int)textSize.Y + 10);
            textPosition = new Vector2(position.X + 5, position.Y + 5);  // Small padding from the edge
        }

        public void Update(GameTime gameTime, Vector2 mousePosition)
        {
            if (backgroundRectangle.Contains(mousePosition))
            {
                hoverTime += gameTime.GetElapsedSeconds();
                    //gameTime.ElapsedGameTime.TotalSeconds;
                if (hoverTime >= tooltipDelay)
                {
                    isVisible = true;
                }
            }
            else
            {
                isVisible = false;
                hoverTime = 0;  // Reset the timer
            }
        }

        public override void Draw(SpriteBatch spriteBatch)//Sprites sprites
        {
            if (isVisible)
            {
                spriteBatch.Draw(backgroundTexture, backgroundRectangle, Color.White);
                spriteBatch.DrawString(font, text, textPosition, Color.Black);
            }
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}