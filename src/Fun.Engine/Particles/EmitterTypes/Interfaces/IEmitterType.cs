using Microsoft.Xna.Framework;

namespace Fun.Engine.Particles.EmitterTypes.Interfaces
{
    public interface IEmitterType
    {
        Vector2 GetParticleDirection();

        Vector2 GetParticlePosition(Vector2 emitterPosition);
    }
}