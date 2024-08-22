using Fun.Engine.Particles.Models;
using Microsoft.Xna.Framework;

namespace Fun.SimpleGame.Particles
{
    public class ParticleState : EmitterParticleState
    {
        protected override int MinLifespan => 30; // equivalent to 30 milliseconds

        protected override int MaxLifespan => 45;

        protected override float Velocity => 2.0f;

        protected override float VelocityDeviation => 0.0f;

        public override float Acceleration => 0.999f;

        public override Vector2 Gravity => Vector2.UnitY;

        protected override float Opacity => 0.4f;

        protected override float OpacityDeviation => 0.1f;

        public override float OpacityFadingRate => 0.92f;

        protected override float Rotation => 0.0f;

        protected override float RotationDeviation => 0.0f;

        protected override float Scale => 0.5f;

        protected override float ScaleDeviation => 0.1f;
    }
}