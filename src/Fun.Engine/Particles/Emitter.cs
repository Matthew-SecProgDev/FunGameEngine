using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Fun.Engine.Entities;
using Fun.Engine.Entities.Interfaces;

namespace Fun.Engine.Particles
{
    public abstract class Emitter : BaseGameObject, IDimensions
    {
        private readonly LinkedList<Particle> _activeParticles = [];
        private readonly LinkedList<Particle> _inactiveParticles = [];
        private readonly Models.EmitterParticleState _emitterParticleState;
        private readonly Color _color;
        private readonly EmitterTypes.Interfaces.IEmitterType _emitterType;
        private readonly int _nbParticleEmittedPerUpdate;
        private readonly int _maxNbParticle;

        public int Age { get; private set; }

        public int Height { get; }

        public int Width { get; }

        protected Emitter(Texture2D texture, Models.EmitterOption option, Vector2 position)
        {
            base.Position = position;
            Texture = texture;

            _color = option.Color;
            _emitterParticleState = option.ParticleState;
            _emitterType = option.Type;
            _nbParticleEmittedPerUpdate = option.NbParticleEmittedPerUpdate;
            _maxNbParticle = option.MaxParticles;
            
            Age = 0;
            Height = texture.Height;
            Width = texture.Width;
        }

        public void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                this.EmitParticles();
            }

            this.UpdateActiveParticles(gameTime);

            Age++;
        }

        public override void Render(Graphics.Sprites sprites)
        {
            var sourceRectangle = new Rectangle(0, 0, Width, Height);

            foreach (var particle in _activeParticles)
            {
                sprites.Draw(Texture,
                    particle.Position + ScreenPosition,
                    sourceRectangle,
                    _color * particle.Opacity,
                    0.0f,
                    Vector2.Zero,
                    particle.Scale,
                    ZIndex);
            }
        }

        private void UpdateActiveParticles(GameTime gameTime)
        {
            var particleNode = _activeParticles.First;
            while (particleNode != null)
            {
                var nextNode = particleNode.Next;
                var stillAlive = particleNode.Value.Update(gameTime);
                if (!stillAlive)
                {
                    _activeParticles.Remove(particleNode);
                    _inactiveParticles.AddLast(particleNode.Value);
                }

                particleNode = nextNode;
            }
        }

        private void EmitParticles()
        {
            // make sure we're not at max particles
            if (_activeParticles.Count >= _maxNbParticle)
            {
                return;
            }

            var maxAmountThatCanBeCreated = _maxNbParticle - _activeParticles.Count;
            var neededParticles = Math.Min(maxAmountThatCanBeCreated, _nbParticleEmittedPerUpdate);

            this.ReuseOrCreateParticles(neededParticles);
        }

        private void ReuseOrCreateParticles(int neededParticles)
        {
            // reuse inactive particles first before creating new ones
            var nbToReuse = Math.Min(_inactiveParticles.Count, neededParticles);
            var nbToCreate = neededParticles - nbToReuse;

            for (var i = 0; i < nbToReuse; i++)
            {
                var particleNode = _inactiveParticles.First!;

                if (particleNode == null)
                {
                    continue;
                }

                this.EmitNewParticle(particleNode.Value);
                _inactiveParticles.Remove(particleNode);
            }

            for (var i = 0; i < nbToCreate; i++)
            {
                this.EmitNewParticle(new Particle());
            }
        }

        private void EmitNewParticle(Particle particle)
        {
            var lifespan = _emitterParticleState.GenerateLifespan();
            var velocity = _emitterParticleState.GenerateVelocity();
            var scale = _emitterParticleState.GenerateScale();
            var rotation = _emitterParticleState.GenerateRotation();
            var opacity = _emitterParticleState.GenerateOpacity();
            var gravity = _emitterParticleState.Gravity;
            var acceleration = _emitterParticleState.Acceleration;
            var opacityFadingRate = _emitterParticleState.OpacityFadingRate;

            var direction = _emitterType.GetParticleDirection();
            var position = _emitterType.GetParticlePosition(Position);

            particle.Activate(lifespan,
                position,
                direction,
                gravity,
                velocity,
                acceleration,
                scale,
                rotation,
                opacity,
                opacityFadingRate);

            _activeParticles.AddLast(particle);
        }
    }
}