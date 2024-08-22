using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.UI.Tooltips.Models
{
    public class TooltipOption
    {
        public required Texture2D BackgroundTexture { get; set; }

        public string Text { get; set; } = string.Empty;

        public required SpriteFont Font { get; set; }

        public Color PenColor { get; set; }
    }
}