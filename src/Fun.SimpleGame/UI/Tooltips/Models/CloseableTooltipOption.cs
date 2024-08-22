using Fun.Engine.UI.Tooltips.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.SimpleGame.UI.Tooltips.Models
{
    public class CloseableTooltipOption : TooltipOption
    {
        public required Texture2D ButtonTexture { get; init; }

        public Vector2 ButtonSize { get; init; }

        public Vector2 ButtonPosition { get; init; }
    }
}