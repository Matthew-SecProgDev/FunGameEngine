using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.UI.Labels
{
    public sealed class Label : BaseUIObject
    {
        private /*readonly*/ string _text;//temporary
        private readonly SpriteFont _font;
        private readonly Color _penColor;

        public Label(Vector2 size, Models.LabelOption option, Vector2 position) : base(size, position)
        {
            _text = option.Text;
            _font = option.Font;
            _penColor = option.PenColor;
        }

        public void SetText(string text)//temporary
        {
            _text = text;
        }

        public override void Render(Graphics.Sprites sprites)
        {
            sprites.DrawString(_font, _text, Position, _penColor);
        }
    }
}