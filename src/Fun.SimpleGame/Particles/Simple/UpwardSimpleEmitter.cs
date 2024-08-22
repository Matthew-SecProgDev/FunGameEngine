using Fun.Engine.Events;
using Fun.Engine.Particles;
using Fun.Engine.Particles.EmitterTypes;
using Fun.Engine.Particles.Models;
using Fun.SimpleGame.States.Gameplay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.SimpleGame.Particles.Simple
{
    public class UpwardSimpleEmitter : Emitter
    {
        private const int NbParticles = 3;
        private const int MaxParticles = 20;
        private static readonly Vector2 Direction = new(0.0f, -1.0f);
        private const float Spread = 2.5f;

        public UpwardSimpleEmitter(Texture2D texture, Vector2 position, Color? color = null)
            : base(texture, CreateOption(color), position)
        {
        }

        public override void OnNotify(BaseGameStateEvent @event)
        {
            if (@event is GameplayEvents.ScreenPositionChanged screenEvent)
            {
                ScreenPosition = screenEvent.Position;
            }
        }

        private static EmitterOption CreateOption(Color? color)
        {
            var option = new EmitterOption
            {
                Type = new ConeEmitterType(Direction, Spread),
                ParticleState = new SimpleParticleState(),
                NbParticleEmittedPerUpdate = NbParticles,
                MaxParticles = MaxParticles
            };

            if (color != null)
            {
                option.Color = color.Value;
            }

            return option;
        }
    }
}