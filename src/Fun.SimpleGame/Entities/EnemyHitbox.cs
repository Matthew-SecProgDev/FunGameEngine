using Fun.Engine;
using Fun.Engine.Entities.Interfaces;
using Fun.Engine.Physics;
using Microsoft.Xna.Framework;

namespace Fun.SimpleGame.Entities
{
    public class EnemyHitbox : BaseGameCollidableObject, IDamageDealer
    {
        private const float Speed = 3.5f;
        private const float LifeSpan = 0.12f;

        private float _elapsedTime;

        public int Damage { get; init; }

        public Controllers.Animation.Enums.EnemyState AttackType { get; init; }

        public void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.GetElapsedSeconds();
            if (_elapsedTime >= LifeSpan)
            {
                Destroy();
                return;
            }

            PositionX = Position.X + Speed * MovementDirection.X;
        }

        public override void Render(Engine.Graphics.Sprites sprites)
        {
            // Ignored
        }
    }
}