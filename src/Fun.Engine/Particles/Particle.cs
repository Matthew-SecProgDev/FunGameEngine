using Microsoft.Xna.Framework;
using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.Engine.Particles
{
    public class Particle
    {
        private int _lifespan; // will tick up every update and monogame updates 60 times per second
        private int _age;
        private Vector2 _direction;
        private Vector2 _gravity;
        private float _velocity;
        private float _acceleration;
        private float _rotation;// do I need this field??
        private float _opacityFadingRate;

        public Vector2 Position { get; private set; }
        public float Scale { get; private set; }
        public float Opacity { get; private set; }

        // this parameters should be moved into a model to improve code quality
        public void Activate(
            int lifespan,
            Vector2 position,
            Vector2 direction,
            Vector2 gravity,
            float velocity,
            float acceleration,
            float scale,
            float rotation,
            float opacity,
            float opacityFadingRate)
        {
            _lifespan = lifespan;
            _direction = direction;
            _velocity = velocity;
            _gravity = gravity;
            _acceleration = acceleration;
            _rotation = rotation;
            _opacityFadingRate = opacityFadingRate;
            _age = 0;

            Position = position;
            Opacity = opacity;
            Scale = scale;
        }

        // returns false if it went past its lifespan
        public bool Update(GameTime gameTime)
        {
            // do I need gameTime or not??!! it must be checked to refactor if needed
            // TODO: update rotation and scale
            _velocity *= _acceleration;
            _direction += _gravity;
            FunMath.Normalize(ref _direction.X, ref _direction.Y);

            var positionDelta = _direction * _velocity;

            Position += positionDelta;

            Opacity *= _opacityFadingRate;

            _age++;

            // return true if particle can stay alive
            return _age < _lifespan;
        }
    }
}