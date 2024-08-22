namespace Fun.SimpleGame.Controllers.Animation.Enums
{
    [Attributes.AnimationState]
    public enum EnemyState
    {
        Nothing = 0,

        Idle = 1,

        WalkForward = 2,

        WalkBackward = 3,

        Dead = 4,

        Attack1 = 5,

        Attack2 = 6,

        Attack3 = 7,

        Hurt = 8
    }
}