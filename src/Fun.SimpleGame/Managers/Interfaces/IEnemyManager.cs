using System.Collections.Generic;
using Fun.Engine.Entities;
using Fun.SimpleGame.Entities;
using Microsoft.Xna.Framework;

namespace Fun.SimpleGame.Managers.Interfaces
{
    public interface IEnemyManager
    {
        event ChildObjectHandler AddObjectHandler;

        event ChildObjectHandler RemoveObjectHandler;

        List<EnemySprite> Enemies { get; }

        List<EnemyHitbox> EnemyHitboxes { get; }

        void Initialize(Vector2 playerPosition);

        void Update(GameTime gameTime);
    }
}