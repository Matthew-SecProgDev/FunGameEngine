using System.Collections.Generic;
using Fun.Engine.Events.Interfaces;
using Fun.SimpleGame.Entities;
using Fun.SimpleGame.States.Gameplay;

namespace Fun.SimpleGame.Managers.CollisionRules
{
    public class HitPlayerByEnemiesAttacks : Interfaces.ICollisionRule
    {
        private readonly IReadOnlyList<EnemyHitbox> _passiveObjects;
        private readonly PlayerSprite _activeObject;
        private readonly IEventBus _eventBus;

        public HitPlayerByEnemiesAttacks(IEventBus eventBus,
            IReadOnlyList<EnemyHitbox> passiveObjects,
            PlayerSprite activeObject)
        {
            _eventBus = eventBus;
            _passiveObjects = passiveObjects;
            _activeObject = activeObject;
        }

        public void Execute()
        {
            var collisionDetector = new Engine.Physics.CollisionDetector<EnemyHitbox, PlayerSprite>(_passiveObjects);
            collisionDetector.DetectCollisions(_activeObject, (hitbox, player) =>
            {
                if (!player.IsAlive)
                {
                    return;
                }

                _eventBus.Publish(new GameplayEvents.ScreenShake());
                player.FreezeAsync();
                hitbox.Owner?.OnNotify(new GameplayEvents.ObjectHitBy
                {
                    Position = player.Position,
                    AttackType = hitbox.AttackType
                });
                player.ApplyDamage(hitbox.Damage);
                hitbox.Destroy();
            });
        }
    }
}