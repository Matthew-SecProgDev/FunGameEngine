using Fun.Engine.Events;
using Fun.Engine.Events.Interfaces;
using Fun.Engine.Particles;
using Fun.Engine.Particles.EmitterTypes;
using Fun.Engine.Particles.Models;
using Fun.SimpleGame.Particles.Simple;
using Fun.SimpleGame.States.Gameplay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.SimpleGame.Particles
{
    public class CircularEmitter : Emitter
    {
        private readonly IEventBus _eventBus = null!;

        public CircularEmitter(Texture2D texture, Vector2 position) 
            : base(texture, CreateOption(), position)
        {
        }

        public override void OnNotify(BaseGameStateEvent @event)
        {
            if (@event is GameplayEvents.ScreenPositionChanged screenEvent)
            {
                ScreenPosition = screenEvent.Position;
            }
        }

        private static EmitterOption CreateOption()
        {
            const int nbParticles = 2;
            const int maxParticles = 200;
            const float radius = 50f;

            return new EmitterOption
            {
                Type = new CircleEmitterType(radius),
                ParticleState = new SimpleParticleState2(),
                NbParticleEmittedPerUpdate = nbParticles,
                MaxParticles = maxParticles
            };
        }
    }
}