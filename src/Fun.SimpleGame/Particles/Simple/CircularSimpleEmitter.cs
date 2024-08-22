using Fun.Engine.Events;
using Fun.Engine.Particles;
using Fun.Engine.Particles.EmitterTypes;
using Fun.Engine.Particles.Models;
using Fun.SimpleGame.States.Gameplay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.SimpleGame.Particles.Simple
{
    // refactor it
    public class CircularSimpleEmitter : Emitter
    {
        private const int NbParticles = 2;
        private const int MaxParticles = 200;
        private const float Radius = 50f;

        public CircularSimpleEmitter(Texture2D texture, Vector2 position)
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
            var option = new EmitterOption
            {
                Type = new CircleEmitterType(Radius),
                ParticleState = new SimpleParticleState2(),
                NbParticleEmittedPerUpdate = NbParticles,
                MaxParticles = MaxParticles
            };

            return option;
        }
    }
}