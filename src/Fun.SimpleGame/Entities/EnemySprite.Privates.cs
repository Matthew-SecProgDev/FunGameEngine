using System;
using System.Threading.Tasks;
using Fun.Engine;
using Fun.Engine.Physics;
using Fun.SimpleGame.Controllers.Animation.Enums;
using Fun.SimpleGame.Controllers.Animation.Models;
using Fun.SimpleGame.Entities.Enums;
using Microsoft.Xna.Framework;
using RNG = Fun.Engine.Utilities.RandomNumberGenerator;
using MathHelper = Fun.Engine.Utilities.MathHelper;

namespace Fun.SimpleGame.Entities
{
    public partial class EnemySprite
    {
        private void PerformMove(float horizontalDistanceToPlayer, float verticalDistanceToPlayer, Vector2 playerPosition)
        {
            var enemyPosX = Position.X;
            var enemyPosY = Position.Y;

            if (!MathHelper.IsNearlyEqual(enemyPosY, playerPosition.Y))
            {
                if (horizontalDistanceToPlayer > LimitedDistanceX * 1.5f)
                {
                    if (enemyPosX > playerPosition.X)
                    {
                        if (MovementDirection == Vector2.UnitX)
                        {
                            MovementDirection = new Vector2(-1f, 0.0f);
                        }

                        enemyPosX -= WalkingSpeed;

                        UpdateCurrentAnimation(EnemyState.WalkForward);
                    }
                    else
                    {
                        if (MovementDirection != Vector2.UnitX)
                        {
                            MovementDirection = Vector2.UnitX;
                        }

                        enemyPosX += WalkingSpeed;

                        UpdateCurrentAnimation(EnemyState.WalkBackward);
                    }
                }
                else
                {
                    if (enemyPosX > playerPosition.X)
                    {
                        if (MovementDirection == Vector2.UnitX)
                        {
                            MovementDirection = new Vector2(-1f, 0.0f);
                        }

                        enemyPosX += WalkingSpeed;

                        UpdateCurrentAnimation(EnemyState.WalkForward);
                    }
                    else
                    {
                        if (MovementDirection != Vector2.UnitX)
                        {
                            MovementDirection = Vector2.UnitX;
                        }

                        enemyPosX -= WalkingSpeed;

                        UpdateCurrentAnimation(EnemyState.WalkBackward);
                    }
                }

                if (enemyPosY > playerPosition.Y)
                {
                    if (verticalDistanceToPlayer > WalkingSpeed)
                    {
                        enemyPosY -= WalkingSpeed;
                    }
                    else
                    {
                        enemyPosY -= verticalDistanceToPlayer;
                    }
                }
                else
                {
                    if (verticalDistanceToPlayer > WalkingSpeed)
                    {
                        enemyPosY += WalkingSpeed;
                    }
                    else
                    {
                        enemyPosY += verticalDistanceToPlayer;
                    }
                }
            }
            else
            {
                if (enemyPosX > playerPosition.X)
                {
                    if (horizontalDistanceToPlayer > WalkingSpeed)
                    {
                        if (MovementDirection == Vector2.UnitX)
                        {
                            MovementDirection = new Vector2(-1f, 0.0f);
                        }

                        enemyPosX -= WalkingSpeed;

                        UpdateCurrentAnimation(EnemyState.WalkBackward);
                    }
                }
                else
                {
                    if (horizontalDistanceToPlayer > WalkingSpeed)
                    {
                        if (MovementDirection != Vector2.UnitX)
                        {
                            MovementDirection = Vector2.UnitX;
                        }

                        enemyPosX += WalkingSpeed;

                        UpdateCurrentAnimation(EnemyState.WalkForward);
                    }
                }
            }

            PositionX = enemyPosX;
            PositionY = enemyPosY;
        }

        private void UpdateAttackState(GameTime gameTime)
        {
            var totalTime = gameTime.GetTotalSeconds();

            // If currently attacking and the idle threshold has passed, reset to idle state
            if (totalTime > _idleThreshold)
            {
                SetNextIdleThreshold(totalTime, 0.49f);
                _currentAnim?.Reset();
                UpdateCurrentAnimation(EnemyState.Idle);
            }
            else if (totalTime - _timeLastAttack > _attackDuration)// Check if the attack duration has elapsed
            {
                _isAttacking = false;
            }
        }

        private void PerformAttack(GameTime gameTime)
        {
            _isAttacking = true;
            _timeLastAttack = gameTime.GetTotalSeconds();

            var (state, delay, damage) = GetRandomAttackInfo();
            SetNextIdleThreshold(_timeLastAttack, delay);
            UpdateCurrentAnimation(state);
            CreateHitboxAsync(damage, state);
        }

        private static (EnemyState State, float Delay, int Damage) GetRandomAttackInfo()
        {
            var rnd = RNG.NextRandom(0, 6);
            return rnd switch
            {
                0 or 3 => (EnemyState.Attack1, 0.35f, 10),
                1 or 4 => (EnemyState.Attack2, 0.42f, 10),
                _ => (EnemyState.Attack3, 0.36f, 15)
            };
        }

        private async void CreateHitboxAsync(int damage, EnemyState state)
        {
            if (state == EnemyState.Attack1)
            {
                _soundEffectManager.PlaySound(EnemySound.Slash);
            }
            else if (state == EnemyState.Attack2)
            {
                _soundEffectManager.PlaySound(EnemySound.Sword);
            }
            else if (state == EnemyState.Attack3)
            {
                _soundEffectManager.PlaySound(EnemySound.SwordD);
            }

            var x = (int)ScreenPosition.X + Position.X + Width - 60;
            var y = (int)ScreenPosition.Y + Position.Y + 25;

            var hitbox = new EnemyHitbox
            {
                AttackType = state,
                Damage = damage,
                Position = new Vector2(x, y),
                MovementDirection = MovementDirection,
                Owner = this
            };

            var cbPosX = x + (MovementDirection == Vector2.UnitX ? -10 : -45);
            var cbPosY = y + 40;
            const int cbWidth = 10;
            const int cbHeight = 10;
            hitbox.AddCollisionBox(new CollisionBox2D(new Vector2(cbPosX, cbPosY), cbWidth, cbHeight));

            await Task.Delay(TimeSpan.FromSeconds(0.2));

            AddObject(hitbox);

            _enemyHitboxes.Add(hitbox);
        }

        private void InitializeBloodSplashEffect()
        {
            const string texturePath = "Textures/Effects/Hitting";
            var effect1 = new PlayerEffect(_resourceManager, texturePath, "enemy_hit")
            {
                Width = 200,
                Height = 200
            };
            effect1.AddFrame(0, 0, 512, 512, 0.01f);
            effect1.AddFrame(512, 0, 512, 512, 0.01f);
            effect1.AddFrame(1024, 0, 512, 512, 0.01f);
            effect1.AddFrame(1536, 0, 512, 512, 0.01f);
            effect1.AddFrame(0, 512, 512, 512, 0.01f);
            effect1.AddFrame(512, 512, 512, 512, 0.01f);
            effect1.AddFrame(1024, 512, 512, 512, 0.01f);
            effect1.AddFrame(1536, 512, 512, 512, 0.01f);
            effect1.AddFrame(0, 1024, 512, 512, 0.01f);
            effect1.AddFrame(512, 1024, 512, 512, 0.01f);
            effect1.AddFrame(1024, 1024, 512, 512, 0.01f);
            effect1.AddFrame(1536, 1024, 512, 512, 0.01f);
            effect1.AddFrame(0, 1536, 512, 512, 0.01f);
            effect1.AddFrame(512, 1536, 512, 512, 0.01f);
            effect1.AddFrame(1024, 1536, 512, 512, 0.01f);
            effect1.AddFrame(1536, 1536, 512, 512, 0.01f);
            effect1.Deactivate();
            _bloodSpatter.Add(EnemyState.Attack1, effect1);
            AddObject(effect1);

            var effect2 = new PlayerEffect(_resourceManager, texturePath, "enemy_hit2", Color.DarkRed)
            {
                Width = 200,
                Height = 200
            };
            effect2.AddFrame(0, 0, 1024, 1024, 0.01f);
            effect2.AddFrame(1024, 0, 1024, 1024, 0.01f);
            effect2.AddFrame(2048, 0, 1024, 1024, 0.01f);
            effect2.AddFrame(3072, 0, 1024, 1024, 0.01f);
            effect2.AddFrame(0, 1024, 1024, 1024, 0.01f);
            effect2.AddFrame(1024, 1024, 1024, 1024, 0.01f);
            effect2.AddFrame(2048, 1024, 1024, 1024, 0.01f);
            effect2.AddFrame(3072, 1024, 1024, 1024, 0.01f);
            effect2.AddFrame(0, 2048, 1024, 1024, 0.01f);
            effect2.AddFrame(1024, 2048, 1024, 1024, 0.01f);
            effect2.AddFrame(2048, 2048, 1024, 1024, 0.01f);
            effect2.AddFrame(3072, 2048, 1024, 1024, 0.01f);
            effect2.AddFrame(0, 3072, 1024, 1024, 0.01f);
            effect2.AddFrame(1024, 3072, 1024, 1024, 0.01f);
            effect2.AddFrame(2048, 3072, 1024, 1024, 0.01f);
            effect2.AddFrame(3072, 3072, 1024, 1024, 0.01f);
            effect2.Deactivate();
            _bloodSpatter.Add(EnemyState.Attack2, effect2);
            AddObject(effect2);

            var effect3 = new PlayerEffect(_resourceManager, texturePath, "enemy_hit3")
            {
                Width = 200,
                Height = 200
            };
            effect3.AddFrame(0, 0, 512, 512, 0.01f);
            effect3.AddFrame(512, 0, 512, 512, 0.01f);
            effect3.AddFrame(1024, 0, 512, 512, 0.01f);
            effect3.AddFrame(1536, 0, 512, 512, 0.01f);
            effect3.AddFrame(0, 512, 512, 512, 0.01f);
            effect3.AddFrame(512, 512, 512, 512, 0.01f);
            effect3.AddFrame(1024, 512, 512, 512, 0.01f);
            effect3.AddFrame(1536, 512, 512, 512, 0.01f);
            effect3.AddFrame(0, 1024, 512, 512, 0.01f);
            effect3.AddFrame(512, 1024, 512, 512, 0.01f);
            effect3.AddFrame(1024, 1024, 512, 512, 0.01f);
            effect3.AddFrame(1536, 1024, 512, 512, 0.01f);
            effect3.AddFrame(0, 1536, 512, 512, 0.01f);
            effect3.AddFrame(512, 1536, 512, 512, 0.01f);
            effect3.AddFrame(1024, 1536, 512, 512, 0.01f);
            effect3.AddFrame(1536, 1536, 512, 512, 0.01f);
            effect3.Deactivate();
            _bloodSpatter.Add(EnemyState.Attack3, effect3);
            AddObject(effect3);
        }

        private void ApplyBloodSplashEffect(Vector2 position, EnemyState attackType)
        {
            if (_bloodSpatter.TryGetValue(attackType, out var effect))
            {
                effect.Reset();
                effect.SetPosition((int)position.X - 45, (int)position.Y - 30);
                effect.Activate();
            }
        }

        private async void ProcessDeathAsync()
        {
            UpdateCurrentAnimation(EnemyState.Dead);

            await Task.Delay(TimeSpan.FromSeconds(1));

            Destroy();
            _soundEffectManager.Dispose();
            _animationController.Clear();
            foreach (var effect in _bloodSpatter.Values)
            {
                effect.Clear();
                RemoveObject(effect);
            }
            _bloodSpatter.Clear();
        }

        private void SetNextIdleThreshold(float totalTime, float delay)
        {
            _idleThreshold = totalTime + delay;
        }

        private void InitializeCollisionBox()
        {
            AddCollisionBox(new CollisionBox2D(new Vector2(Position.X + 38, Position.Y), 60, 110));
        }

        private void UpdateCurrentAnimation(EnemyState state)
        {
            (Texture, _currentAnim) = _animationController.GetCurrentAnimationCycle(state).Value;
        }

        private void InitializeAnimationManager()
        {
            var option = new AnimationOption
            {
                FrameWidth = 73,
                FrameHeight = 72,
                BasePath = "Textures/Enemies/Enemy1"
            };
            _animationController = new Controllers.Animation.AnimationController<EnemyState>(_resourceManager, option);

            _animationController.BindAnimationCycle(
                _animationController.CreateAnimationCycle(
                    "idle",
                    new AnimationCycleOption
                    {
                        FramePosX = [30],
                        FramePosY = [55],
                        FrameTime = 0.0f,
                        IsLooping = false
                    }),
                EnemyState.Nothing);

            _animationController.BindAnimationCycle(
                _animationController.CreateAnimationCycle(
                    "idle",
                    new AnimationCycleOption
                    {
                        FramePosX = [30, 158, 286, 414, 542, 670, 798],
                        FramePosY = [55, 55, 55, 55, 55, 55, 55],
                        FrameTime = 0.07f,
                        IsLooping = true
                    }),
                EnemyState.Idle);

            _animationController.BindAnimationCycle(
                _animationController.CreateAnimationCycle(
                    "walk",
                    new AnimationCycleOption
                    {
                        FramePosX = [30, 158, 286, 414, 542, 670, 798],
                        FramePosY = [55, 55, 55, 55, 55, 55, 55],
                        FrameTime = 0.09f,
                        IsLooping = true
                    }),
                EnemyState.WalkForward);

            _animationController.BindAnimationCycle(
                _animationController.CreateAnimationCycle(
                    "attack1",
                    new AnimationCycleOption
                    {
                        FramePosX = [30, 158, 286, 414, 542],
                        FramePosY = [55, 55, 55, 55, 55],
                        FrameTime = 0.07f,
                        IsLooping = true
                    }),
                EnemyState.Attack1);

            _animationController.BindAnimationCycle(
                _animationController.CreateAnimationCycle(
                    "attack2",
                    new AnimationCycleOption
                    {
                        FramePosX = [30, 158, 286, 414, 542, 670],
                        FramePosY = [55, 55, 55, 55, 55, 55],
                        FrameTime = 0.07f,
                        IsLooping = true
                    }),
                EnemyState.Attack2);

            _animationController.BindAnimationCycle(
                _animationController.CreateAnimationCycle(
                    "attack3",
                    new AnimationCycleOption
                    {
                        FramePosX = [30, 158, 286, 414],
                        FramePosY = [55, 55, 55, 55],
                        FrameTime = 0.1f,
                        IsLooping = true
                    }),
                EnemyState.Attack3);

            _animationController.BindAnimationCycle(
                _animationController.CreateAnimationCycle(
                    "hurt",
                    new AnimationCycleOption
                    {
                        FramePosX = [30, 158],
                        FramePosY = [55, 55],
                        FrameTime = 0.08f,
                        IsLooping = false
                    }),
                EnemyState.Hurt);

            _animationController.BindAnimationCycle(
                _animationController.CreateAnimationCycle(
                    "dead",
                    new AnimationCycleOption
                    {
                        FramePosX = [30, 158, 286, 414],
                        FramePosY = [55, 55, 55, 55],
                        FrameTime = 0.07f,
                        IsLooping = false
                    }),
                EnemyState.Dead);

            _animationController.BindReversedAnimationCycle(EnemyState.WalkForward, EnemyState.WalkBackward);
        }
    }
}