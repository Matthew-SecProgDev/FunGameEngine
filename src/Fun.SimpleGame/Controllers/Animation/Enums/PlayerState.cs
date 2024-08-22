namespace Fun.SimpleGame.Controllers.Animation.Enums
{
    [Attributes.AnimationState]
    public enum PlayerState
    {
        Idle = 0,

        WalkForward = 1,

        WalkBackward = 2,

        Dead = 3,

        Attack1 = 4,

        Attack2 = 5,

        Attack3 = 6,

        Shot = 7,

        Hurt = 8
    }
}