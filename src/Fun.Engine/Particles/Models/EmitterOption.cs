using Microsoft.Xna.Framework;

namespace Fun.Engine.Particles.Models
{
    public class EmitterOption
    {
        public required EmitterTypes.Interfaces.IEmitterType Type { get; set; }

        public required EmitterParticleState ParticleState { get; set; }

        public required int NbParticleEmittedPerUpdate { get; set; }

        public required int MaxParticles { get; set; }

        public Color Color { get; set; } = Color.White;
    }
}