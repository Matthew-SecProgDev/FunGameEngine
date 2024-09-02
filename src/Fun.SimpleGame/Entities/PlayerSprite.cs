using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fun.Engine.Animations;
using Fun.Engine.Audio;
using Fun.Engine.Audio.Models;
using Fun.Engine.Entities.Interfaces;
using Fun.Engine.Events;
using Fun.Engine.Events.Interfaces;
using Fun.Engine.Physics;
using Fun.Engine.Resources;
using Fun.SimpleGame.Controllers.Animation;
using Fun.SimpleGame.Controllers.Animation.Enums;
using Fun.SimpleGame.Entities.Enums;
using Fun.SimpleGame.States.Gameplay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.SimpleGame.Entities
{
    public partial class PlayerSprite(IResourceManager resourceManager, IEventBus eventBus)
        : BaseGameCollidableObject, IDimensions, IHealthEntity
    {
        private const float Speed = 190f;

        private EnemyEffect _bloodSpatter = null!;

        private IAnimationController<PlayerState> _animationController = null!;

        private Animation _currentAnim = null!;

        private TimeSpan _lastArrowShotAt;
        private bool _isShootingArrow;
        private bool _isAllowedThrowArrow;

        private bool _isAllowedAttack;
        private TimeSpan _lastAttack1At;
        private bool _isAttacking1;
        private TimeSpan _lastAttack2At;
        private bool _isAttacking2;
        private TimeSpan _lastAttack3At;
        private bool _isAttacking3;

        private bool _isIdle = false;
        private Vector2 _screenPosition = Vector2.Zero;
        private Vector2 _previousScreenPosition = Vector2.Zero;

        private readonly SoundEffectManager _soundEffectManager = new();

        public bool IsAlive => Health > 0;

        public bool IsFrozen { get; private set; }

        public List<ArrowSprite> Arrows { get; } = [];

        public List<PlayerHitbox> Hitboxes { get; } = [];

        public int Width { get; init; }

        public int Height { get; init; }

        public int Health { get; private set; } = 500;

        public override void Initialize()
        {
            InitializeCollisionBox();
            InitializeAnimationManager();
            InitializeBloodSplashEffect();

            const string footstepPath = "Audio/Sounds/footstep";
            var audioSettingsFootstep = new AudioSettings(0.2f, 0f, 0f);
            var audioTrackFootstep = new AudioTrack(resourceManager.LoadSound(footstepPath), audioSettingsFootstep);
            _soundEffectManager.RegisterSound(GlobalSound.Footstep, audioTrackFootstep);

            var weaponSettingsSlash = new AudioSettings(0.4f, 0f, 0f);

            const string bowPath = "Audio/Sounds/slingshot";
            var audioTrackBow = new AudioTrack(resourceManager.LoadSound(bowPath), weaponSettingsSlash);
            _soundEffectManager.RegisterSound(PlayerSound.Bow, audioTrackBow);

            const string knifeDrawPath = "Audio/Sounds/knife-draw";
            var audioTrackKnifeDraw = new AudioTrack(resourceManager.LoadSound(knifeDrawPath), weaponSettingsSlash);
            _soundEffectManager.RegisterSound(PlayerSound.KnifeDraw, audioTrackKnifeDraw);

            const string swingKnifePath = "Audio/Sounds/swing-knife";
            var audioTrackSwingKnife = new AudioTrack(resourceManager.LoadSound(swingKnifePath), weaponSettingsSlash);
            _soundEffectManager.RegisterSound(PlayerSound.SwingKnife, audioTrackSwingKnife);

            const string hitBowPath = "Audio/Sounds/knife-throw";
            var audioTrackHitBow = new AudioTrack(resourceManager.LoadSound(hitBowPath), weaponSettingsSlash);
            _soundEffectManager.RegisterSound(PlayerSound.HitBow, audioTrackHitBow);
        }

        public void StopMoving()
        {
            _currentAnim?.Reset();

            UpdateCurrentAnimation(PlayerState.Idle);
        }

        public void TryMoveLeft(float elapsedSeconds)
        {
            eventBus.Publish(new GameplayEvents.GoingToLocation(Position, DirectionType.Left)
            {
                ElapsedTime = elapsedSeconds
            });
        }

        public void TryMoveRight(float elapsedSeconds)
        {
            eventBus.Publish(new GameplayEvents.GoingToLocation(Position, DirectionType.Right)
            {
                ElapsedTime = elapsedSeconds
            });
        }

        public void TryMoveUp(float elapsedSeconds)
        {
            eventBus.Publish(new GameplayEvents.GoingToLocation(Position, DirectionType.Up)
            {
                DestinationPosition = new Vector2(Position.X, Position.Y + Speed * elapsedSeconds)
            });
        }

        public void TryMoveDown(float elapsedSeconds)
        {
            eventBus.Publish(new GameplayEvents.GoingToLocation(Position, DirectionType.Down)
            {
                DestinationPosition = new Vector2(Position.X, Position.Y - Speed * elapsedSeconds)
            });
        }

        public void HandlePlayerCollisionPush()
        {
        }

        public void MoveUp(float x, float y)
        {
            if (IsPositionChanged(x, y))
            {
                _isShootingArrow = false;
                _isAttacking1 = false;
                _isAttacking2 = false;
                _isAttacking3 = false;

                _soundEffectManager.PlaySound(GlobalSound.Footstep);
                ChangeCurrentMovementAnimation(DirectionType.Up);

                PositionX = x;
                PositionY = y;

                eventBus.Publish(new GameplayEvents.PlayerMoved(Position));
                _isIdle = false;
            }
            else if(!_isIdle)
            {
                StopMoving();
                _isIdle = true;
            }
        }

        public void MoveDown(float x, float y)
        {
            if (IsPositionChanged(x, y))
            {
                _isShootingArrow = false;
                _isAttacking1 = false;
                _isAttacking2 = false;
                _isAttacking3 = false;

                _soundEffectManager.PlaySound(GlobalSound.Footstep);
                ChangeCurrentMovementAnimation(DirectionType.Down);

                PositionX = x;
                PositionY = y;

                eventBus.Publish(new GameplayEvents.PlayerMoved(Position));
                _isIdle = false;
            }
            else if (!_isIdle)
            {
                StopMoving();
                _isIdle = true;
            }
        }

        public void MoveRight(float x, float y)
        {
            if (IsPositionChanged(x, y))
            {
                _isShootingArrow = false;
                _isAttacking1 = false;
                _isAttacking2 = false;
                _isAttacking3 = false;

                _soundEffectManager.PlaySound(GlobalSound.Footstep);
                ChangeCurrentMovementAnimation(DirectionType.Right);
                ChangeCurrentDirection(DirectionType.Right);

                PositionX = x;
                PositionY = y;

                eventBus.Publish(new GameplayEvents.PlayerMoved(Position));
                _isIdle = false;
            }
            else if (!_isIdle)
            {
                StopMoving();
                _isIdle = true;
            }
        }

        public void MoveLeft(float x, float y)
        {
            if (IsPositionChanged(x, y))
            {
                _isShootingArrow = false;
                _isAttacking1 = false;
                _isAttacking2 = false;
                _isAttacking3 = false;

                _soundEffectManager.PlaySound(GlobalSound.Footstep);
                ChangeCurrentMovementAnimation(DirectionType.Left);
                ChangeCurrentDirection(DirectionType.Left);

                PositionX = x;
                PositionY = y;

                eventBus.Publish(new GameplayEvents.PlayerMoved(Position));
                _isIdle = false;
            }
            else if (!_isIdle)
            {
                StopMoving();
                _isIdle = true;
            }
        }

        public override void OnNotify(BaseGameStateEvent @event)
        {
            if (@event is GameplayEvents.ScreenPositionChanged screenEvent)
            {
                _previousScreenPosition = _screenPosition;
                _screenPosition = screenEvent.Position;
            }
            else if (@event is GameplayEvents.ObjectHitBy hitEvent)
            {
                ApplyBloodSplashEffect(hitEvent.Position);
            }
        }

        public async void FreezeAsync()
        {
            IsFrozen = true;

            await Task.Delay(TimeSpan.FromSeconds(0.25));

            IsFrozen = false;
        }

        public void ApplyDamage(int damageValue)
        {
            UpdateCurrentAnimation(PlayerState.Hurt);

            Health -= damageValue;

            eventBus.Publish(new GameplayEvents.PlayerHit(damageValue));

            if (!IsAlive)
            {
                ProcessDeathAsync();
            }
        }

        public void PerformAttack(GameTime gameTime, PlayerAttackType type)
        {
            switch (type)
            {
                case PlayerAttackType.Attack1:
                    Attack_1(gameTime);
                    break;
                case PlayerAttackType.Attack2:
                    Attack_2(gameTime);
                    break;
                case PlayerAttackType.Attack3:
                    Attack_3(gameTime);
                    break;
                case PlayerAttackType.Shoot:
                    Attack_4(gameTime);
                    break;
                default:
                    throw new ArgumentException("Invalid attack type", nameof(type));
            }
        }

        public void Update(GameTime gameTime)
        {
            UpdateAttacking(gameTime);

            RegulateAttackingRate(gameTime);

            _bloodSpatter.Update(gameTime);

            _currentAnim?.Update(gameTime);

            CleanDestroyed();
        }

        public override void Render(Engine.Graphics.Sprites sprites)
        {
            if (Destroyed)
            {
                return;
            }

            var currentFrame = _currentAnim?.CurrentFrame;
            if (currentFrame == null)
            {
                UpdateCurrentAnimation(PlayerState.Idle);
                currentFrame = _currentAnim.CurrentFrame;
            }

            var destRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            if (MovementDirection == Vector2.UnitX)
            {
                sprites.Draw(Texture,
                    destRectangle,
                    currentFrame.SourceRectangle,
                    Color.White);
            }
            else
            {
                sprites.Draw(Texture,
                    destRectangle,
                    currentFrame.SourceRectangle,
                    Color.White,
                    SpriteEffects.FlipHorizontally);
            }
        }
    }
}