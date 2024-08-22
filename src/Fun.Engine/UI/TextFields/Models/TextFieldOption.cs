using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.UI.TextFields.Models
{
    public sealed class TextFieldOption
    {
        public required Texture2D Texture { get; set; }

        public Color PenColor { get; set; }

        public required SpriteFont Font { get; set; }
    }
}