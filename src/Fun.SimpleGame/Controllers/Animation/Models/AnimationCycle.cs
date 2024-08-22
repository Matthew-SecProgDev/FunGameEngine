using Microsoft.Xna.Framework.Graphics;

namespace Fun.SimpleGame.Controllers.Animation.Models
{
    public class AnimationCycle
    {
        public required Texture2D Texture { get; init; }

        public required Engine.Animations.Animation Animation { get; init; }
    }
}