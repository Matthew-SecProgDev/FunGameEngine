using System;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.SimpleGame.Controllers.Animation
{
    public interface IAnimationController<in TState> where TState : Enum
    {
        Models.AnimationCycle CreateAnimationCycle(string animationName, Models.AnimationCycleOption option);

        void BindAnimationCycle(Models.AnimationCycle animationCycle, TState state);

        void BindReversedAnimationCycle(TState originalState, TState newState);

        (Texture2D, Engine.Animations.Animation)? GetCurrentAnimationCycle(TState state);

        void Clear();
    }
}