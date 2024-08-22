using Fun.Engine.Entities.Interfaces;
using Fun.Engine.Physics;

namespace Fun.SimpleGame.Entities
{
    public class PlayerHitbox : BaseGameCollidableObject, IDamageDealer
    {
        private const float Speed = 2.0f;
        private const float LifeSpan = 0.14f;

        private float _elapsedTime;

        public int Damage { get; init; } = 15;

        public void Update(float elapsedTime)
        {
            _elapsedTime += elapsedTime;
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