using System;
using System.Threading.Tasks;
using Fun.Engine;
using Fun.Engine.Physics;
using Fun.SimpleGame.Controllers.Animation;
using Fun.SimpleGame.Controllers.Animation.Enums;
using Fun.SimpleGame.Controllers.Animation.Models;
using Fun.SimpleGame.Entities.Enums;
using Fun.SimpleGame.States.Gameplay;
using Microsoft.Xna.Framework;
using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.SimpleGame.Entities
{
    public partial class PlayerSprite
    {
        private bool IsPositionChanged(float x, float y)
        {
            var previousPosX = _previousScreenPosition.X + Position.X;
            var previousPosY = _previousScreenPosition.Y + Position.Y;
            var currentPosX = _screenPosition.X + x;
            var currentPosY = _screenPosition.Y + y;

            if (!FunMath.IsNearlyEqual(previousPosX, currentPosX))
            {
                return true;
            }

            if (!FunMath.IsNearlyEqual(previousPosY, currentPosY))
            {
                return true;
            }

            return false;
        }

        private void UpdateAttacking(GameTime gameTime)
        {
            var elapsedTime = gameTime.GetElapsedSeconds();
            for (var i = 0; i < Hitboxes.Count; i++)
            {
                if (Hitboxes[i] is { IsActive: true, Destroyed: false })
                {
                    Hitboxes[i].Update(elapsedTime);
                }
            }

            for (var i = 0; i < Arrows.Count; i++)
            {
                if (Arrows[i] is { IsActive: true, Destroyed: false })
                {
                    Arrows[i].Update(elapsedTime);
                }
            }
        }

        private void RegulateAttackingRate(GameTime gameTime)
        {
            if (_isShootingArrow)
            {
                RegulateAttack(
                    gameTime.TotalGameTime - _lastArrowShotAt,
                    1.1,
                    ref _isShootingArrow,
                    0.5,
                    _isAllowedThrowArrow,
                    CreateArrow
                );
            }
            else if (_isAttacking1)
            {
                RegulateAttack(
                    gameTime.TotalGameTime - _lastAttack1At,
                    0.45,
                    ref _isAttacking1,
                    0.3,
                    _isAllowedAttack,
                    CreateHitbox
                );
            }
            else if (_isAttacking2)
            {
                RegulateAttack(
                    gameTime.TotalGameTime - _lastAttack2At,
                    0.4,
                    ref _isAttacking2,
                    0.3,
                    _isAllowedAttack,
                    CreateHitbox
                );
            }
            else if (_isAttacking3)
            {
                RegulateAttack(
                    gameTime.TotalGameTime - _lastAttack3At,
                    0.4,
                    ref _isAttacking3,
                    0.3,
                    _isAllowedAttack,
                    CreateHitbox
                );
            }
        }

        private static void RegulateAttack(
            TimeSpan offset, 
            double duration, 
            ref bool isAttacking, 
            double delayedAction, 
            bool isAllowed, 
            Action action)
        {
            if (offset >= TimeSpan.FromSeconds(duration))
            {
                isAttacking = false;
            }
            else if (isAllowed && offset >= TimeSpan.FromSeconds(delayedAction))
            {
                action();
            }
        }

        private void CleanDestroyed()
        {
            for (var i = 0; i < Hitboxes.Count; i++)
            {
                if (Hitboxes[i].Destroyed)
                {
                    RemoveObject(Hitboxes[i]);
                    Hitboxes.Remove(Hitboxes[i]);
                }
            }

            // arrows must be removed when they are out of the screen or camera range

            for (var i = 0; i < Arrows.Count; i++)
            {
                if (Arrows[i].Destroyed)
                {
                    RemoveObject(Arrows[i]);
                    Arrows.Remove(Arrows[i]);
                }
            }
        }

        private void ChangeCurrentMovementAnimation(DirectionType type)
        {
            switch (type)
            {
                case DirectionType.Up:
                case DirectionType.Right:
                case DirectionType.Left:
                    UpdateCurrentAnimation(PlayerState.WalkForward);
                    break;
                case DirectionType.Down:
                    UpdateCurrentAnimation(PlayerState.WalkBackward);
                    break;
                default:
                    throw new ArgumentException("Invalid direction type", nameof(type));
            }
        }

        private void ChangeCurrentDirection(DirectionType type)
        {
            switch (type)
            {
                case DirectionType.Left:
                    if (MovementDirection == Vector2.UnitX)
                    {
                        MovementDirection = new Vector2(-1.0f, 0.0f);
                        eventBus.Publish(new GameplayEvents.PlayerDirectionChanged(MovementDirection));
                    }
                    break;

                case DirectionType.Right:
                    if (MovementDirection != Vector2.UnitX)
                    {
                        MovementDirection = Vector2.UnitX;
                        eventBus.Publish(new GameplayEvents.PlayerDirectionChanged(MovementDirection));
                    }
                    break;

                default:
                    throw new ArgumentException("Invalid direction type", nameof(type));
            }
        }

        private void Attack_1(GameTime gameTime)
        {
            if (_isAttacking1)
            {
                return;
            }

            _isShootingArrow = false;
            _isAttacking2 = false;
            _isAttacking3 = false;

            _soundEffectManager.PlaySound(PlayerSound.HitBow);

            UpdateCurrentAnimation(PlayerState.Attack1);

            _isAllowedAttack = true;
            _isAttacking1 = true;
            _lastAttack1At = gameTime.TotalGameTime;

        }

        private void Attack_2(GameTime gameTime)
        {
            if (_isAttacking2)
            {
                return;
            }

            _isShootingArrow = false;
            _isAttacking1 = false;
            _isAttacking3 = false;

            _soundEffectManager.PlaySound(PlayerSound.KnifeDraw);

            UpdateCurrentAnimation(PlayerState.Attack2);

            _isAllowedAttack = true;
            _isAttacking2 = true;
            _lastAttack2At = gameTime.TotalGameTime;
        }

        private void Attack_3(GameTime gameTime)
        {
            if (_isAttacking3)
            {
                return;
            }

            _isShootingArrow = false;
            _isAttacking1 = false;
            _isAttacking2 = false;

            _soundEffectManager.PlaySound(PlayerSound.SwingKnife);

            UpdateCurrentAnimation(PlayerState.Attack3);

            _isAllowedAttack = true;
            _isAttacking3 = true;
            _lastAttack3At = gameTime.TotalGameTime;
        }

        private void CreateHitbox()
        {
            var x = Position.X + Width - 27;
            var y = Position.Y + 25;

            var hitbox = new PlayerHitbox
            {
                Position = new Vector2(x, y),
                MovementDirection = MovementDirection,
                Owner = this
            };

            var isMovingRight = MovementDirection == Vector2.UnitX;
            var cbPosition = new Vector2(x + (isMovingRight ? 0f : -83f), y + 40f);
            const int cbWidth = 10;
            const int cbHeight = 10;
            hitbox.AddCollisionBox(new CollisionBox2D(cbPosition, cbWidth, cbHeight));

            AddObject(hitbox);

            Hitboxes.Add(hitbox);

            _isAllowedAttack = false;
            Console.WriteLine("attack..........");
        }

        private void Attack_4(GameTime gameTime)
        {
            if (_isShootingArrow)
            {
                return;
            }

            _isAttacking1 = false;
            _isAttacking2 = false;
            _isAttacking3 = false;

            _soundEffectManager.PlaySound(PlayerSound.Bow);
            UpdateCurrentAnimation(PlayerState.Shot);

            _isAllowedThrowArrow = true;
            _isShootingArrow = true;
            _lastArrowShotAt = gameTime.TotalGameTime;
        }

        private void CreateArrow()
        {
            //TODO: it should be refactored
            var isMovingRight = MovementDirection == Vector2.UnitX;
            var x = isMovingRight ? Position.X + Width / 2f : Position.X + 20f;
            var y = Position.Y + 60f;

            var arrow = new ArrowSprite(resourceManager)
            {
                Position = new Vector2(x, y),
                MovementDirection = MovementDirection,
                Damage = 20,
                Owner = this
            };

            var cbPosition = new Vector2(x + (isMovingRight ? 38f : 2f), y + 21f);
            const int cbWidth = 9;
            const int cbHeight = 6;
            arrow.AddCollisionBox(new CollisionBox2D(cbPosition, cbWidth, cbHeight));

            AddObject(arrow);

            Arrows.Add(arrow);

            _isAllowedThrowArrow = false;
        }

        private async void ProcessDeathAsync()
        {
            UpdateCurrentAnimation(PlayerState.Dead);

            await Task.Delay(TimeSpan.FromSeconds(1));

            Destroy();
            CleanDestroyed();
            _soundEffectManager.Dispose();
            _animationController.Clear();
            _bloodSpatter.Clear();
            RemoveObject(_bloodSpatter);
        }

        private void InitializeBloodSplashEffect()
        {
            const string texturePath = "Textures/Effects/Hitting";

            _bloodSpatter = new EnemyEffect(resourceManager, texturePath, "player_hit", Color.Purple)
            {
                Width = 200,
                Height = 200
            };
            _bloodSpatter.AddFrame(0, 0, 512, 512, 0.01f);
            _bloodSpatter.AddFrame(512, 0, 512, 512, 0.01f);
            _bloodSpatter.AddFrame(1024, 0, 512, 512, 0.01f);
            _bloodSpatter.AddFrame(1536, 0, 512, 512, 0.01f);
            _bloodSpatter.AddFrame(0, 512, 512, 512, 0.01f);
            _bloodSpatter.AddFrame(512, 512, 512, 512, 0.01f);
            _bloodSpatter.AddFrame(1024, 512, 512, 512, 0.01f);
            _bloodSpatter.AddFrame(1536, 512, 512, 512, 0.01f);
            _bloodSpatter.AddFrame(0, 1024, 512, 512, 0.01f);
            _bloodSpatter.AddFrame(512, 1024, 512, 512, 0.01f);
            _bloodSpatter.AddFrame(1024, 1024, 512, 512, 0.01f);
            _bloodSpatter.AddFrame(1536, 1024, 512, 512, 0.01f);
            _bloodSpatter.AddFrame(0, 1536, 512, 512, 0.01f);
            _bloodSpatter.AddFrame(512, 1536, 512, 512, 0.01f);
            _bloodSpatter.AddFrame(1024, 1536, 512, 512, 0.01f);
            _bloodSpatter.AddFrame(1536, 1536, 512, 512, 0.01f);

            _bloodSpatter.Deactivate();

            AddObject(_bloodSpatter);
        }

        private void ApplyBloodSplashEffect(Vector2 position)
        {
            _bloodSpatter.Reset();
            _bloodSpatter.SetPosition((int)position.X - 45, (int)position.Y - 30);
            _bloodSpatter.Activate();
        }

        private void UpdateCurrentAnimation(PlayerState state)
        {
            (Texture, _currentAnim) = _animationController.GetCurrentAnimationCycle(state).Value;
        }

        private void InitializeCollisionBox()
        {
            AddCollisionBox(new CollisionBox2D(new Vector2(Position.X + 32f, Position.Y), 62f, 110f));
        }

        private void InitializeAnimationManager()
        {
            var option = new AnimationOption
            {
                FrameWidth = 73,
                FrameHeight = 72,
                BasePath = "Textures/Player"
            };
            _animationController = new AnimationController<PlayerState>(resourceManager, option);

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
                PlayerState.Idle);

            _animationController.BindAnimationCycle(
                _animationController.CreateAnimationCycle(
                    "walk",
                    new AnimationCycleOption
                    {
                        FramePosX = [30, 158, 286, 414, 542, 670, 798, 926],
                        FramePosY = [55, 55, 55, 55, 55, 55, 55, 55],
                        FrameTime = 0.09f,
                        IsLooping = true
                    }),
                PlayerState.WalkForward);

            _animationController.BindAnimationCycle(
                _animationController.CreateAnimationCycle(
                    "shot1",
                    new AnimationCycleOption
                    {
                        FramePosX = [30, 158, 286, 414, 542, 670, 798, 926, 1054, 1182, 1310, 1438, 1566, 1694, 1822],
                        FramePosY = [55, 55, 55, 55, 55, 55, 55, 55, 55, 55, 55, 55, 55, 55, 55],
                        FrameTime = 0.05f,
                        IsLooping = true
                    }),
                PlayerState.Shot);

            _animationController.BindAnimationCycle(
                _animationController.CreateAnimationCycle(
                    "attack1",
                    new AnimationCycleOption
                    {
                        FramePosX = [30, 158, 286, 414, 542],
                        FramePosY = [55, 55, 55, 55, 55],
                        FrameTime = 0.08f,
                        IsLooping = true
                    }),
                PlayerState.Attack1);

            _animationController.BindAnimationCycle(
                _animationController.CreateAnimationCycle(
                    "attack2",
                    new AnimationCycleOption
                    {
                        FramePosX = [26, 154, 282, 410],
                        FramePosY = [55, 55, 55, 55],
                        FrameTime = 0.1f,
                        IsLooping = true
                    }),
                PlayerState.Attack2);

            _animationController.BindAnimationCycle(
                _animationController.CreateAnimationCycle(
                    "attack3",
                    new AnimationCycleOption
                    {
                        FramePosX = [36, 164, 292],
                        FramePosY = [55, 55, 55],
                        FrameTime = 0.15f,
                        IsLooping = true
                    }),
                PlayerState.Attack3);

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
                PlayerState.Hurt);

            _animationController.BindAnimationCycle(
                _animationController.CreateAnimationCycle(
                    "dead",
                    new AnimationCycleOption
                    {
                        FramePosX = [30, 158, 286, 414, 542],
                        FramePosY = [55, 55, 55, 55, 55],
                        FrameTime = 0.1f,
                        IsLooping = false
                    }),
                PlayerState.Dead);

            _animationController.BindReversedAnimationCycle(PlayerState.WalkForward, PlayerState.WalkBackward);
        }
    }
}