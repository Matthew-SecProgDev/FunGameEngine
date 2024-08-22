using Microsoft.Xna.Framework;

namespace Fun.Engine.Animations.Models
{
    public class AnimationFrame(Rectangle source, float frameTime)
    {
        public Rectangle SourceRectangle { get; } = source;

        public float FrameTime { get; } = frameTime;
    }
}