using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fun.Engine;
using Fun.Engine.Animations;
using Fun.Engine.Audio;
using Fun.Engine.Audio.Models;
using Fun.Engine.Entities.Interfaces;
using Fun.Engine.Events.Interfaces;
using Fun.Engine.Physics;
using Fun.Engine.Resources;
using Fun.SimpleGame.Controllers.Animation.Enums;
using Fun.SimpleGame.Entities.Enums;
using Fun.SimpleGame.Particles.Simple;
using Fun.SimpleGame.States.Gameplay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaMathHelper = Microsoft.Xna.Framework.MathHelper;

namespace Fun.SimpleGame.Entities
{
    public partial class EnemySprite : BaseGameCollidableObject, IDimensions, IHealthEntity
    {
        private const float WalkingSpeed = 1.2f;
        private const float LimitedDistanceX = 70f;
        private const float FadeDuration = 4.0f; // Fade-in duration

        private readonly Dictionary<EnemyState, PlayerEffect> _bloodSpatter;
        private readonly float _attackDuration = (float)TimeSpan.FromSeconds(1.5).TotalSeconds;
        private readonly SoundEffectManager _soundEffectManager;
        private readonly List<EnemyHitbox> _enemyHitboxes;
        private readonly IResourceManager _resourceManager;
        private readonly IEventBus _eventBus;

        private Controllers.Animation.IAnimationController<EnemyState> _animationController;
        private Animation _currentAnim;

        private float _idleThreshold;
        private float _timeLastAttack;
        private bool _isAttacking;

        private SimpleEmitterGenerator _emitterGenerator;
        private float _opacity; // Start with 0% transparency
        private float _elapsedTime;
        
        private Vector2 _playerPosition;
        private bool _playerDetected;
        private bool _canAttack;
        private bool _canWalk;
        private int _health;

        public Guid Id { get; } = Guid.NewGuid();

        public readonly string Name = "Gravemaster";

        public bool IsAlive => Health > 0;

        public bool IsFrozen { get; private set; }

        public int Width { get; init; }

        public int Height { get; init; }

        public int Health
        {
            get => _health; 
            init => _health = value;
        }

        public Configurations.Enemy.Enums.EnemyRole Role { get; init; }//refactor it

        public EnemySprite(
            IResourceManager resourceManager,
            IEventBus eventBus,
            List<EnemyHitbox> enemyHitboxes, 
            float? attackDuration = null)
        {
            _resourceManager = resourceManager;
            _eventBus = eventBus;
            _enemyHitboxes = enemyHitboxes;//I must use another way instead of this, refactor it
            _soundEffectManager = new SoundEffectManager();

            if (attackDuration != null)
            {
                _attackDuration = (float)TimeSpan.FromSeconds(attackDuration.Value).TotalSeconds;//refactor it
            }

            _bloodSpatter = new Dictionary<EnemyState, PlayerEffect>(3);
        }

        public override void Initialize()
        {
            this.InitializeCollisionBox();
            this.InitializeAnimationManager();
            this.InitializeBloodSplashEffect();

            const string footstepPath = "Audio/Sounds/footstep";
            var audioSettingsFootstep = new AudioSettings(0.2f, 0f, 0f);
            var audioTrackFootstep = new AudioTrack(_resourceManager.LoadSound(footstepPath), audioSettingsFootstep);
            _soundEffectManager.RegisterSound(GlobalSound.Footstep, audioTrackFootstep);

            var weaponSettingsSlash = new AudioSettings(0.4f, 0f, 0f);

            const string slashPath = "Audio/Sounds/slash";
            var audioTrackSlash = new AudioTrack(_resourceManager.LoadSound(slashPath), weaponSettingsSlash);
            _soundEffectManager.RegisterSound(EnemySound.Slash, audioTrackSlash);

            const string swordPath = "Audio/Sounds/sword";
            var audioTrackSword = new AudioTrack(_resourceManager.LoadSound(swordPath), weaponSettingsSlash);
            _soundEffectManager.RegisterSound(EnemySound.Sword, audioTrackSword);

            const string sword2Path = "Audio/Sounds/sword2";
            var audioTrackSword2 = new AudioTrack(_resourceManager.LoadSound(sword2Path), weaponSettingsSlash);
            _soundEffectManager.RegisterSound(EnemySound.SwordD, audioTrackSword2);

            _emitterGenerator = new SimpleEmitterGenerator(_resourceManager);
            _emitterGenerator.AddObjectHandler += this.AddObject;
            _emitterGenerator.RemoveObjectHandler += this.RemoveObject;
            var x = (int)ScreenPosition.X + (int)Position.X;
            var y = (int)ScreenPosition.Y + (int)Position.Y;
            var position = new Vector2(x + 58, y + Height);
            _emitterGenerator.Initialize(Color.Aquamarine, position, MovementDirection);
        }

        public override void OnNotify(Engine.Events.BaseGameStateEvent @event)
        {
            if (@event is GameplayEvents.ScreenPositionChanged screenEvent)
            {
                this.ScreenPosition = screenEvent.Position;
            }
            else if (@event is GameplayEvents.PlayerMoved playerMovementEvent)
            {
                _playerPosition = playerMovementEvent.Position;
            }
            else if (@event is GameplayEvents.ObjectHitBy hitEvent)
            {
                this.ApplyBloodSplashEffect(hitEvent.Position, hitEvent.AttackType);
            }
        }

        public async void FreezeAsync()
        {
            this.IsFrozen = true;

            await Task.Delay(TimeSpan.FromSeconds(0.25));

            this.IsFrozen = false;
        }

        public void ApplyDamage(int damageValue)
        {
            this.UpdateCurrentAnimation(EnemyState.Hurt);

            _health -= damageValue;

            _eventBus.Publish(new GameplayEvents.EnemyHit(this.Id, damageValue));

            if (!_playerDetected)
            {
                _playerDetected = true;
            }

            if (!IsAlive)
            {
                this.ProcessDeathAsync();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsAlive)
            {
                if (_opacity < 1f)
                {
                    _elapsedTime += gameTime.GetElapsedSeconds();

                    // Fade-in logic
                    if (_elapsedTime < EnemySprite.FadeDuration)
                    {
                        _opacity = XnaMathHelper.Lerp(0.0f, 1f, _elapsedTime / EnemySprite.FadeDuration);
                    }
                    else
                    {
                        _opacity = 1f;
                    }

                    _emitterGenerator.Update(gameTime);
                }

                this.UpdateAttackState(gameTime);

                if (_playerPosition != Vector2.Zero)//refactor it
                {
                    var playerPosition = new Vector2(-1 * this.ScreenPosition.X + _playerPosition.X, _playerPosition.Y);

                    if (!_playerDetected && _elapsedTime >= EnemySprite.FadeDuration &&
                        _emitterGenerator is { Status: ParticleGeneratorStatus.Completed })
                    {
                        _emitterGenerator.AddObjectHandler -= AddObject;
                        _emitterGenerator.RemoveObjectHandler -= RemoveObject;
                        _emitterGenerator = null;
                        if (Vector2.Distance(Position, playerPosition) < 250f)
                        {
                            _playerDetected = true;
                        }
                    }

                    if (this.IsFrozen)
                    {
                        return;
                    }

                    if (_playerDetected)
                    {
                        var verticalDistanceToPlayer = XnaMathHelper.Distance(Position.Y, playerPosition.Y);
                        var horizontalDistanceToPlayer = XnaMathHelper.Distance(Position.X, playerPosition.X);

                        if ((int)Position.Y == (int)playerPosition.Y && horizontalDistanceToPlayer <= LimitedDistanceX)
                        {
                            _canWalk = false;
                            _canAttack = true;
                        }
                        else
                        {
                            _canAttack = false;
                            _canWalk = true;
                        }

                        if (_canWalk)
                        {
                            PerformMove(horizontalDistanceToPlayer, verticalDistanceToPlayer, playerPosition);
                            _soundEffectManager.PlaySound(GlobalSound.Footstep);
                        }

                        if (_canAttack && !_isAttacking)
                        {
                            PerformAttack(gameTime);
                        }
                    }

                    foreach (var effect in _bloodSpatter.Values)
                    {
                        effect.Update(gameTime);
                    }
                }
            }

            _currentAnim?.Update(gameTime);
        }

        public override void Render(Engine.Graphics.Sprites sprites)
        {
            if (Destroyed)
            {
                return;
            }

            var x = (int)ScreenPosition.X + (int)Position.X;
            var y = (int)ScreenPosition.Y + (int)Position.Y;
            var destRectangle = new Rectangle(x, y, Width, Height);

            if (MovementDirection == Vector2.UnitX)
            {
                sprites.Draw(Texture,
                    destRectangle,
                    _currentAnim.CurrentFrame.SourceRectangle,
                    Color.White * _opacity);
            }
            else
            {
                sprites.Draw(Texture,
                    destRectangle,
                    _currentAnim.CurrentFrame.SourceRectangle,
                    Color.White * _opacity,
                    SpriteEffects.FlipHorizontally);
            }
        }
    }
}