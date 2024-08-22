using Fun.Engine.Resources;
using Microsoft.Xna.Framework;

namespace Fun.SimpleGame.Entities
{
    public class EnemyEffect : PlayerEffect
    {
        public EnemyEffect(
            IResourceManager resourceManager,
            string texturePath,
            string textureName,
            Color? color = null) : base(resourceManager, texturePath, textureName, color)
        {
        }

        public override void OnNotify(Engine.Events.BaseGameStateEvent @event)
        {
            if (@event is States.Gameplay.GameplayEvents.ScreenPositionChanged screenEvent)
            {
                ScreenPosition = screenEvent.Position;
            }
        }

        public override void Render(Engine.Graphics.Sprites sprites)
        {
            var x = (int)ScreenPosition.X + (int)Position.X;
            var y = (int)ScreenPosition.Y + (int)Position.Y;
            var destRectangle = new Rectangle(x, y, Width, Height);

            sprites.Draw(Texture,
                destRectangle,
                Animation.CurrentFrame.SourceRectangle,
                Color);
        }
    }
}