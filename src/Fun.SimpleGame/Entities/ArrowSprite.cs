using Fun.Engine.Entities.Interfaces;
using Fun.Engine.Physics;
using Fun.Engine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.SimpleGame.Entities
{
    public class ArrowSprite : BaseGameCollidableObject, IDamageDealer
    {
        private const float Speed = 2.0f;

        public int Damage { get; init; }

        public ArrowSprite(IResourceManager resourceManager)
        {
            Texture = resourceManager.LoadTexture("Textures/Player/arrow");
        }

        public void Update(float elapsedTime)
        {
            PositionX = Position.X + Speed * MovementDirection.X * elapsedTime * 1000;//refactor it
        }

        public override void Render(Engine.Graphics.Sprites sprites)
        {
            if (MovementDirection == Vector2.UnitX)
            {
                sprites.Draw(Texture, Position, null, Color.White);
            }
            else
            {
                sprites.Draw(Texture, Position, null, Color.White, SpriteEffects.FlipHorizontally);
            }
        }
    }
}