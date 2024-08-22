using Fun.Engine.Particles.EmitterTypes.Interfaces;
using Microsoft.Xna.Framework;
using RNG = Fun.Engine.Utilities.RandomNumberGenerator;
using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.Engine.Particles.EmitterTypes
{
    public class ConeEmitterType(Vector2 direction, float spread) : IEmitterType
    {
        public Vector2 GetParticleDirection()
        {
            var angle = MathF.Atan2(direction.Y, direction.X);
            var newAngle = RNG.NextRandom(angle - spread / 2.0f, angle + spread / 2.0f);

            var particleDirection = new Vector2(MathF.Cos(newAngle), MathF.Sin(newAngle));
            FunMath.Normalize(ref particleDirection.X, ref particleDirection.Y);

            return particleDirection;
        }

        public Vector2 GetParticlePosition(Vector2 emitterPosition)
        {
            // return the same position for this type of emitter, but otherwise we could tweak this to start particles a bit further
            // away from the center of the cone.

            return new Vector2(emitterPosition.X, emitterPosition.Y);
        }
    }
}