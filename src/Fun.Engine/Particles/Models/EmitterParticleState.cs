using Microsoft.Xna.Framework;
using RNG = Fun.Engine.Utilities.RandomNumberGenerator;

namespace Fun.Engine.Particles.Models
{
    public abstract class EmitterParticleState
    {
        protected abstract int MinLifespan { get; }
        protected abstract int MaxLifespan { get; }

        protected abstract float Velocity { get; }
        protected abstract float VelocityDeviation { get; }
        public abstract float Acceleration { get; }
        public abstract Vector2 Gravity { get; }

        protected abstract float Opacity { get; }
        protected abstract float OpacityDeviation { get; }
        public abstract float OpacityFadingRate { get; }

        protected abstract float Rotation { get; }
        protected abstract float RotationDeviation { get; }

        protected abstract float Scale { get; }
        protected abstract float ScaleDeviation { get; }

        public int GenerateLifespan()
        {
            return RNG.NextRandom(MinLifespan, MaxLifespan);
        }

        public float GenerateVelocity()
        {
            return EmitterParticleState.GenerateFloat(Velocity, VelocityDeviation);
        }

        public float GenerateOpacity()
        {
            return EmitterParticleState.GenerateFloat(Opacity, OpacityDeviation);
        }

        public float GenerateRotation()
        {
            return EmitterParticleState.GenerateFloat(Rotation, RotationDeviation);
        }

        public float GenerateScale()
        {
            return EmitterParticleState.GenerateFloat(Scale, ScaleDeviation);
        }

        private static float GenerateFloat(float startN, float deviation)
        {
            var halfDeviation = deviation / 2.0f;

            return RNG.NextRandom(startN - halfDeviation, startN + halfDeviation);
        }
    }
}