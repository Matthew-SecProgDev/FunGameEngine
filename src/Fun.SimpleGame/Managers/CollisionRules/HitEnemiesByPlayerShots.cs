using System.Collections.Generic;
using Fun.SimpleGame.Entities;

namespace Fun.SimpleGame.Managers.CollisionRules
{
    public class HitEnemiesByPlayerShots : Interfaces.ICollisionRule
    {
        private readonly IReadOnlyList<ArrowSprite> _passiveObjects;
        private readonly IReadOnlyList<EnemySprite> _activeObjects;

        public HitEnemiesByPlayerShots(
            IReadOnlyList<ArrowSprite> passiveObjects, 
            IReadOnlyList<EnemySprite> activeObjects)
        {
            _passiveObjects = passiveObjects;
            _activeObjects = activeObjects;
        }

        public void Execute()
        {
            var collisionDetector = new Engine.Physics.CollisionDetector<ArrowSprite, EnemySprite>(_passiveObjects);
            collisionDetector.DetectCollisions(_activeObjects, (arrow, enemy) =>
            {
                if (!enemy.IsAlive)
                {
                    return;
                }

                enemy.FreezeAsync();
                arrow.Owner?.OnNotify(new States.Gameplay.GameplayEvents.ObjectHitBy
                {
                    Position = enemy.Position
                });
                //temporary
                enemy.ApplyDamage(arrow.Damage);
                arrow.Destroy();
            });
        }
    }
}