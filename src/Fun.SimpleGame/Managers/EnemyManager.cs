using System;
using System.Collections.Generic;
using Fun.Engine.Entities;
using Fun.Engine.Events.Interfaces;
using Fun.Engine.Resources;
using Fun.SimpleGame.Configurations.Enemy.Enums;
using Fun.SimpleGame.Configurations.Enemy.Models;
using Fun.SimpleGame.Entities;
using Fun.SimpleGame.States.Gameplay;
using Microsoft.Xna.Framework;
using RNG = Fun.Engine.Utilities.RandomNumberGenerator;

namespace Fun.SimpleGame.Managers
{
    public class EnemyManager : Interfaces.IEnemyManager
    {
        private readonly IResourceManager _resourceManager;
        private readonly IEventBus _eventBus;
        private readonly EnemyConfig _enemyConfig;
        private readonly Engine.Graphics.Camera _camera;

        private Vector2 _playerPosition;
        private Vector2 _screenPosition = Vector2.Zero;

        public event ChildObjectHandler AddObjectHandler;

        public event ChildObjectHandler RemoveObjectHandler;

        public List<EnemySprite> Enemies { get; } = [];

        public List<EnemyHitbox> EnemyHitboxes { get; } = [];

        public EnemyManager(Engine.Graphics.Camera camera, IResourceManager resourceManager, IEventBus eventBus)
        {
            _camera = camera;
            _resourceManager = resourceManager;
            _eventBus = eventBus;

            _enemyConfig = Configurations.Enemy.EnemyConfigParser.Parse("EnemyConfig.xml");//temporary
        }

        public void Initialize(Vector2 playerPosition)
        {
            _playerPosition = playerPosition;

            CreateEnemiesImmediately();

            _eventBus.Subscribe<GameplayEvents.PlayerMoved>(playerMovedHandler =>
            {
                _playerPosition = playerMovedHandler.Position;
            });
            _eventBus.Subscribe<GameplayEvents.ScreenPositionChanged>(screenChangedHandler =>
            {
                _screenPosition = screenChangedHandler.Position;
            });
        }

        public void Update(GameTime gameTime)
        {
            CreateEnemiesGradually();

            UpdateEnemies(gameTime);

            CleanDestroyed();
        }

        private void CreateEnemiesImmediately()
        {
            _camera.GetExtents(out float width, out float height);
            var halfHeight = height * 0.5f;
            var halfWidth = width * 0.5f;

            var counter = 0;
            foreach (var enemy in _enemyConfig.EnemySpawns)
            {
                if (!enemy.BuildTime.StartImmediately)
                {
                    break;
                }

                CreateEnemy(enemy, halfWidth, halfHeight);
                counter++;
            }

            if (counter > 0)
            {
                _enemyConfig.EnemySpawns.RemoveRange(0, counter);
                counter = 0;
            }

            foreach (var enemyGroup in _enemyConfig.EnemyGroups)
            {
                if (!enemyGroup.EnemySpawns[0].BuildTime.StartImmediately)
                {
                    break;
                }

                foreach (var enemy in enemyGroup.EnemySpawns)
                {
                    CreateEnemy(enemy, halfWidth, halfHeight);
                }

                counter++;
            }

            if (counter > 0)
            {
                _enemyConfig.EnemyGroups.RemoveRange(0, counter);
            }

            _enemyConfig.EnemySpawns.RemoveAll(i => i.BuildTime.StartImmediately);
            //_enemyConfig.EnemyGroups.RemoveAll(i => i.EnemySpawns.Any(j => j.BuildTime.StartImmediately));
        }

        private void CreateEnemiesGradually()
        {
            _camera.GetExtents(out float width, out float height);
            var halfHeight = height * 0.5f;
            var halfWidth = width * 0.5f;

            var screenPositionX = -1 * (int)_screenPosition.X;//- halfWidth * 0.15f;

            var counter = 0;

            foreach (var enemy in _enemyConfig.EnemySpawns)
            {
                if (enemy.BuildTime.TriggerPoint > screenPositionX)
                {
                    break;
                }

                CreateEnemy(enemy, halfWidth, halfHeight);
                counter++;
            }

            if (counter > 0)
            {
                _enemyConfig.EnemySpawns.RemoveRange(0, counter);
                counter = 0;
            }

            foreach (var enemyGroup in _enemyConfig.EnemyGroups)
            {
                if (enemyGroup.EnemySpawns[0].BuildTime.TriggerPoint > screenPositionX)
                {
                    break;
                }

                foreach (var enemy in enemyGroup.EnemySpawns)
                {
                    CreateEnemy(enemy, halfWidth, halfHeight);
                }

                counter++;
            }

            if (counter > 0)
            {
                _enemyConfig.EnemyGroups.RemoveRange(0, counter);
            }
        }

        private void CreateEnemy(EnemySpawn enemy, float width, float height)
        {
            Vector2 position;

            if (enemy.Position.Area.HasValue)
            {
                GenerateRandomPosition(enemy.Position.Area.Value, width, height, out position);
            }
            else
            {
                position.X = _playerPosition.X -_screenPosition.X + enemy.Position.Offset;
                position.Y = _playerPosition.Y;
            }

            SpawnEnemy(enemy.Role, enemy.Health, enemy.Active, position);
        }

        private void GenerateRandomPosition(SpawnArea area, float width, float height, out Vector2 position)
        {
            //I can draw a rectangle for debugging

            var screenPositionX = -_screenPosition.X;
            if (area == SpawnArea.BottomLeft)
            {
                position.X = RNG.NextRandom((-width + screenPositionX) * 0.98f , - 1f + screenPositionX);
                position.Y = RNG.NextRandom(-height * 0.67f, -1f);
            }
            else if (area == SpawnArea.BottomRight)
            {
                position.X = RNG.NextRandom(1f + screenPositionX, (width + screenPositionX) * 0.98f);
                position.Y = RNG.NextRandom(-height * 0.67f, -1f);
            }
            else if (area == SpawnArea.TopLeft)
            {
                position.X = RNG.NextRandom((-width + screenPositionX) * 0.98f, -1f + screenPositionX);
                position.Y = RNG.NextRandom(1f, height * 0.19f);
            }
            else if (area == SpawnArea.TopRight)
            {
                position.X = RNG.NextRandom(1f + screenPositionX, (width + screenPositionX) * 0.98f);
                position.Y = RNG.NextRandom(1f, height * 0.19f);
            }
            else
            {
                throw new ArgumentException("Invalid SpawnArea value", nameof(area));
            }
        }

        private void SpawnEnemy(EnemyRole role, int health, bool active, Vector2 position)
        {
            var rnd = RNG.NextRandom(0, 4);
            var actionTime = RNG.NextRandom(1.0f, 1.7f);
            var enemy = new EnemySprite(_resourceManager, _eventBus, EnemyHitboxes, actionTime)
            {
                ScreenPosition = _screenPosition,
                MovementDirection = rnd is 0 or 2 ? new Vector2(-1f, 0.0f) : Vector2.UnitX,
                Health = health,
                Role = role,
                Height = 130,
                Width = 145
            };

            enemy.AddObjectHandler += HandleAddObject;
            enemy.RemoveObjectHandler += HandleRemoveObject;
            enemy.Position = position;

            HandleAddObject(enemy);
            Enemies.Add(enemy);

            _eventBus.Publish(new GameplayEvents.EnemyCreated
            {
                Id = enemy.Id,
                Name = enemy.Name,
                Health = enemy.Health
            });
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            for (var i = 0; i < EnemyHitboxes.Count; i++)
            {
                if (!EnemyHitboxes[i].Destroyed && EnemyHitboxes[i].IsActive)
                {
                    EnemyHitboxes[i].Update(gameTime);
                }
            }

            for (var i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Update(gameTime);
            }
        }

        private void CleanDestroyed()
        {
            for (var i = 0; i < EnemyHitboxes.Count; i++)
            {
                if (EnemyHitboxes[i].Destroyed)
                {
                    HandleRemoveObject(EnemyHitboxes[i]); //check it
                    EnemyHitboxes.Remove(EnemyHitboxes[i]); //check it
                }
            }

            for (var i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].Destroyed)
                {
                    Enemies[i].AddObjectHandler -= HandleAddObject;
                    Enemies[i].RemoveObjectHandler -= HandleRemoveObject;
                    HandleRemoveObject(Enemies[i]);
                    Enemies.Remove(Enemies[i]);
                }
            }
        }

        private void HandleAddObject(BaseObject baseObject)
        {
            AddObjectHandler?.Invoke(baseObject);
        }

        private void HandleRemoveObject(BaseObject baseObject)
        {
            RemoveObjectHandler?.Invoke(baseObject);
        }
    }
}