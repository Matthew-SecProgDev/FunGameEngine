using Fun.Engine.Particles.EmitterTypes.Interfaces;
using Microsoft.Xna.Framework;
using RNG = Fun.Engine.Utilities.RandomNumberGenerator;
using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.Engine.Particles.EmitterTypes
{
    public class CircleEmitterType(float radius) : IEmitterType
    {
        public Vector2 GetParticleDirection()
        {
            return Vector2.Zero;
        }

        public Vector2 GetParticlePosition(Vector2 emitterPosition)
        {
            var newAngle = RNG.NextRandom(0f, 2 * Microsoft.Xna.Framework.MathHelper.Pi);
            var positionVector = new Vector2(MathF.Cos(newAngle), MathF.Sin(newAngle));
            FunMath.Normalize(ref positionVector.X, ref positionVector.Y);

            var distance = RNG.NextRandom(0f, radius);
            var position = positionVector * distance;

            var x = emitterPosition.X + position.X;
            var y = emitterPosition.Y + position.Y;

            return new Vector2(x, y);
        }
    }
}