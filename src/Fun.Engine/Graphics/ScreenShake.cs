using Microsoft.Xna.Framework;
using RNG = Fun.Engine.Utilities.RandomNumberGenerator;
using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.Engine.Graphics
{
    public sealed class ScreenShake
    {
        private float _duration;
        private float _magnitude;
        private float _elapsed;

        public const float MinMagnitude = 1.5f;
        public const float MaxMagnitude = 5f;
        public const float MinDuration = 0.3f;
        public const float MaxDuration = 0.8f;

        public Vector2 ShakeOffset { get; private set; }

        public void StartShake(float duration, float magnitude)
        {
            duration = FunMath.Clamp(duration, MinDuration, MaxDuration);
            magnitude = FunMath.Clamp(magnitude, MinMagnitude, MaxMagnitude);

            _duration = duration;
            _magnitude = magnitude;
            _elapsed = 0f;
        }

        public void Update(GameTime gameTime)
        {
            if (_elapsed < _duration)
            {
                _elapsed += gameTime.GetElapsedSeconds();
                var progress = _elapsed / _duration;
                var dampening = 1f - progress;

                ShakeOffset = new Vector2(
                    RNG.NextRandom(-1f, 0.9f) * _magnitude * dampening,
                    RNG.NextRandom(-1f, 0.9f) * _magnitude * dampening
                );
            }
            else
            {
                ShakeOffset = Vector2.Zero;
            }
        }
    }
}