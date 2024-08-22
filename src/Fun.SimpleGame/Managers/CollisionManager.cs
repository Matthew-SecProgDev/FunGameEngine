using System;
using System.Collections.Generic;
using Fun.SimpleGame.Managers.Interfaces;

namespace Fun.SimpleGame.Managers
{
    public class CollisionManager : ICollisionManager
    {
        private Dictionary<Guid, ICollisionRule> _collisionRules;

        public CollisionManager()
        {
            _collisionRules = new Dictionary<Guid, ICollisionRule>();
        }

        public void AddCollisionRule(ICollisionRule collisionRule)
        {
            _collisionRules.Add(Guid.NewGuid(), collisionRule);
        }

        public void Update()
        {
            foreach (var collisionRule in _collisionRules.Values)
            {
                collisionRule.Execute();
            }
        }
    }
}