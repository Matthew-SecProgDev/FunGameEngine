using System;
using System.Collections.Generic;
using System.IO;
using Fun.Engine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.SimpleGame.Controllers.Animation
{
    public class AnimationController<TState> : IAnimationController<TState> where TState : Enum
    {
        private readonly Dictionary<TState, Models.AnimationCycle> _cycle = [];
        private readonly IResourceManager _resourceManager;
        private readonly Models.AnimationOption _option;

        public AnimationController(IResourceManager resourceManager, Models.AnimationOption option)
        {
            if (!typeof(TState).IsDefined(typeof(Attributes.AnimationStateAttribute), false))
            {
                throw new InvalidOperationException($"{typeof(TState).Name} is not a valid animation state enum.");
            }

            _resourceManager = resourceManager;
            _option = option;

            ArgumentException.ThrowIfNullOrWhiteSpace(_option.BasePath);
        }

        public Models.AnimationCycle CreateAnimationCycle(string animationName, Models.AnimationCycleOption option)
        {
            if (option.FramePosX.Length != option.FramePosY.Length)
            {
                throw new ArgumentException("The lengths of FramePosX and FramePosY arrays must be equal.", nameof(option));
            }

            var animation = new Engine.Animations.Animation(option.IsLooping);
            for (var i = 0; i < option.FramePosX.Length; i++)
            {
                var srcRectangle = new Rectangle(option.FramePosX[i], option.FramePosY[i], _option.FrameWidth, _option.FrameHeight);
                animation.AddFrame(srcRectangle, option.FrameTime);
            }

            return new Models.AnimationCycle
            {
                Texture = _resourceManager.LoadTexture(Path.Combine(_option.BasePath, animationName)),
                Animation = animation
            };
        }

        public void BindAnimationCycle(Models.AnimationCycle animationCycle, TState state)
        {
            if (!_cycle.TryAdd(state, animationCycle))
            {
                throw new InvalidOperationException($"An animation cycle is already bound to the state {state}.");
            }
        }

        public void BindReversedAnimationCycle(TState originalState, TState newState)
        {
            if (_cycle.TryGetValue(originalState, out var animation))
            {
                var reversedCycle = new Models.AnimationCycle
                {
                    Animation = animation.Animation.ReverseAnimation,
                    Texture = animation.Texture
                };

                if (!_cycle.TryAdd(newState, reversedCycle))
                {
                    throw new InvalidOperationException($"An animation cycle is already bound to the state {newState}.");
                }
            }
            else
            {
                throw new KeyNotFoundException($"No animation cycle found for the state {originalState}.");
            }
        }

        public (Texture2D, Engine.Animations.Animation)? GetCurrentAnimationCycle(TState state)
        {
            if (_cycle.TryGetValue(state, out var animationCycle))
            {
                return (animationCycle.Texture, animationCycle.Animation);
            }

            return null;
        }

        public void Clear()
        {
            foreach (var animationCycle in _cycle.Values)
            {
                animationCycle.Animation.Clear();
            }

            _cycle.Clear();
        }
    }
}