using Fun.Engine.Particles.Models;
using Microsoft.Xna.Framework;

namespace Fun.SimpleGame.Particles.Simple
{
    public class SimpleParticleState : EmitterParticleState
    {
        protected override int MinLifespan => 60;

        protected override int MaxLifespan => 75;   // equivalent to 1.2 seconds

        protected override float Velocity => 120.1f;

        protected override float VelocityDeviation => 150.0f;

        public override float Acceleration => 0.3f;

        public override Vector2 Gravity => Vector2.Zero;

        protected override float Opacity => 0.7f;

        protected override float OpacityDeviation => 0.1f;

        public override float OpacityFadingRate => 1.0f;

        protected override float Rotation => 0.0f;

        protected override float RotationDeviation => 0.0f;

        protected override float Scale => 0.025f;

        protected override float ScaleDeviation => 0.04f;
    }
}