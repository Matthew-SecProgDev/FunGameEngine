using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.UI.Labels.Models
{
    public sealed class LabelOption
    {
        public required string Text { get; set; }

        public Color PenColor { get; set; }

        public required SpriteFont Font { get; set; }
    }
}