using System.Collections.Generic;
using Fun.SimpleGame.Entities;

namespace Fun.SimpleGame.Managers.CollisionRules
{
    public class HitEnemiesByPlayerAttacks : Interfaces.ICollisionRule
    {
        private readonly IReadOnlyList<PlayerHitbox> _passiveObjects;
        private readonly IReadOnlyList<EnemySprite> _activeObjects;

        public HitEnemiesByPlayerAttacks(
            IReadOnlyList<PlayerHitbox> passiveObjects, 
            IReadOnlyList<EnemySprite> activeObjects)
        {
            _passiveObjects = passiveObjects;
            _activeObjects = activeObjects;
        }

        public void Execute()
        {
            var collisionDetector = new Engine.Physics.CollisionDetector<PlayerHitbox, EnemySprite>(_passiveObjects);
            collisionDetector.DetectCollisions(_activeObjects, (hitbox, enemy) =>
            {
                if (!enemy.IsAlive)
                {
                    return;
                }

                enemy.FreezeAsync();
                hitbox.Owner?.OnNotify(new States.Gameplay.GameplayEvents.ObjectHitBy
                {
                    Position = enemy.Position
                });
                enemy.ApplyDamage(hitbox.Damage);
                hitbox.Destroy();
            });
        }
    }
}