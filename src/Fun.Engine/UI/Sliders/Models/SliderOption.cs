using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.UI.Sliders.Models
{
    public class SliderOption
    {
        public required Texture2D BackgroundTexture { get; set; }

        public required Texture2D HandleTexture { get; set; }

        public Vector2 HandleSize { get; set; }

        public required Texture2D FillTexture { get; set; }

        public float FillHeight { get; set; }

        public float InitialValue { get; set; }

        public float MinValue { get; set; }

        public float MaxValue { get; set; }
    }
}