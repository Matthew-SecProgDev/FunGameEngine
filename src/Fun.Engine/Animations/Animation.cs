using Microsoft.Xna.Framework;

namespace Fun.Engine.Animations
{
    public class Animation(bool looping)
    {
        private readonly List<Models.AnimationFrame> _frames = new(30);
        private float _lifespan = -1f;
        private float _animationAge;

        public float Lifespan
        {
            get
            {
                if (_lifespan < 0f)
                {
                    _lifespan = 0f;
                    foreach (var frame in _frames)
                    {
                        _lifespan += frame.FrameTime;
                    }
                }

                return _lifespan;
            }
        }

        public Models.AnimationFrame? CurrentFrame
        {
            get
            {
                Models.AnimationFrame? currentFrame = null;

                var accumulatedTime = 0f;
                foreach (var frame in _frames)
                {
                    if (accumulatedTime + frame.FrameTime >= _animationAge)
                    {
                        currentFrame = frame;
                        break;
                    }

                    accumulatedTime += frame.FrameTime;
                }

                return currentFrame ?? _frames.LastOrDefault();
            }
        }

        public Animation ReverseAnimation
        {
            get
            {
                var reversedAnimation = new Animation(looping);

                for (var i = _frames.Count - 1; i >= 0; i--)
                {
                    reversedAnimation.AddFrame(_frames[i].SourceRectangle, _frames[i].FrameTime);
                }

                return reversedAnimation;
            }
        }

        public void AddFrame(Rectangle sourceRectangle, float frameTime)
        {
            _frames.Add(new Models.AnimationFrame(sourceRectangle, frameTime));
        }

        public void Update(GameTime gameTime)
        {
            _animationAge += gameTime.GetElapsedSeconds();

            if (looping && _animationAge > Lifespan)
            {
                _animationAge = 0f;
            }
        }

        public void Reset()
        {
            _animationAge = 0f;
        }

        public void Clear()
        {
            _frames.Clear();
        }
    }
}