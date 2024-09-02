using Microsoft.Xna.Framework.Graphics;

namespace Fun.SimpleGame.UI.HUDs.Models
{
    public class HealthBarOption
    {
        public required Texture2D BackgroundTexture { get; init; }

        public required Texture2D FillTexture { get; init; }
    }
}