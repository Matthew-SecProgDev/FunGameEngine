namespace Fun.SimpleGame.Managers.Interfaces
{
    public interface ICollisionManager
    {
        void AddCollisionRule(ICollisionRule collisionRule);

        void Update();
    }
}