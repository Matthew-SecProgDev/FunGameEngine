namespace Fun.SimpleGame.Controllers.Animation.Models
{
    public class AnimationCycleOption
    {
        public required int[] FramePosX { get; init; }

        public required int[] FramePosY { get; init; }

        public float FrameTime { get; init; }

        public bool IsLooping { get; init; }
    }
}