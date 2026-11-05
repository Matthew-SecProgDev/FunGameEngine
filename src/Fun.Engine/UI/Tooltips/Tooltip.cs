using Fun.Engine.Graphics;
using Fun.Engine.UI.Tooltips.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.UI.Tooltips
{
    public class SimpleTooltip : BaseTooltip
    {
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
            protected set => _tooltipBounds = value;
        }

        public string Text { get; private set; }

        public Vector2 TextPosition
        {
            get => _textPosition; 
            protected set => _textPosition = value;
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
}