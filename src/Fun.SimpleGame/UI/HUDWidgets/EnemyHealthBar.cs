using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fun.Engine;
using Fun.Engine.UI;
using Fun.SimpleGame.UI.HUDWidgets.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.SimpleGame.UI.HUDWidgets
{
    public class EnemyHealthBar : BaseUIObject
    {
        private readonly Dictionary<Guid, EnemyHealthBarStatus> _enemyHealthStatusMap = [];
        private readonly float _lifeSpan = (float)TimeSpan.FromSeconds(1.5).TotalSeconds;
        private readonly Texture2D _fillTexture;
        private readonly object _locker = new();

        private float _elapsedTime;
        private Guid _currentEnemyId;

        public EnemyHealthBar(Vector2 size, HealthBarOption option, Vector2 position) : base(size, position)
        {
            Texture = option.BackgroundTexture;
            _fillTexture = option.FillTexture;
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }

            _elapsedTime += gameTime.GetElapsedSeconds();
            if (_elapsedTime > _lifeSpan)
            {
                Deactivate();

                lock (_locker)
                {
                    if (_enemyHealthStatusMap.TryGetValue(_currentEnemyId, out var healthStatus) &&
                        FunMath.IsNearlyZero(healthStatus.Percentage))
                    {
                        _enemyHealthStatusMap.Remove(_currentEnemyId);
                    }
                }
            }
        }

        public override void OnNotify(Engine.Events.BaseGameStateEvent @event)
        {
            if (@event is States.Gameplay.GameplayEvents.EnemyHit hitEvent)
            {
                _elapsedTime = 0f;

                lock (_locker)
                {
                    if (!_enemyHealthStatusMap.ContainsKey(hitEvent.Id))
                    {
                        return;
                    }

                    _currentEnemyId = hitEvent.Id;

                    if (!IsActive)
                    {
                        Activate();
                    }

                    UpdateHealthPercentageAsync(hitEvent.Value, hitEvent.Id);
                }
            }
            else if (@event is States.Gameplay.GameplayEvents.EnemyCreated enemyCreatedEvent)
            {
                AddEntity(enemyCreatedEvent.Id, enemyCreatedEvent.Name, enemyCreatedEvent.Health);
            }
        }

        public override void Render(Engine.Graphics.Sprites sprites)
        {
            var positionX = (int)Position.X;
            var positionY = (int)Position.Y;
            var sizeY = (int)Size.Y;

            // it should display name pic of each enemy
            var backgroundRect = new Rectangle(positionX, positionY, (int)Size.X, sizeY);
            sprites.Draw(Texture, backgroundRect, null, Color.White);

            lock (_locker)
            {
                if (_enemyHealthStatusMap.TryGetValue(_currentEnemyId, out var healthStatus))
                {
                    var width = (int)(Size.X * healthStatus.Percentage);
                    var fillRect = new Rectangle(positionX, positionY, width, sizeY);
                    sprites.Draw(_fillTexture, fillRect, null, GetFillColor(healthStatus.Percentage));
                }
            }
        }

        private void AddEntity(Guid id, string name, int health)
        {
            lock (_locker)
            {
                _enemyHealthStatusMap[id] = new EnemyHealthBarStatus
                {
                    Name = name,
                    Health = health,
                    Percentage = 1f
                };
            }
        }

        private async void UpdateHealthPercentageAsync(int damageValue, Guid enemyId)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.2));

            lock (_locker)
            {
                if (_enemyHealthStatusMap.TryGetValue(enemyId, out var healthStatus))
                {
                    if (damageValue >= healthStatus.Health)
                    {
                        healthStatus.Percentage = 0f;
                    }
                    else
                    {
                        var damagePercentage = (float)damageValue / healthStatus.Health;
                        healthStatus.Percentage = FunMath.Clamp(healthStatus.Percentage - damagePercentage, 0f, 1f);
                    }
                }
            }
        }

        private static Color GetFillColor(float percentage)
        {
            if (percentage >= 0.9f)
                return Color.Green;//it's temporarily set

            if (percentage is >= 0.8f and < 0.9f)
                return Color.GreenYellow;

            if (percentage is >= 0.6f and < 0.8f)
                return Color.LightYellow;

            if (percentage is >= 0.4f and < 0.6f)
                return Color.Orange;

            return Color.Red;
        }
    }
}