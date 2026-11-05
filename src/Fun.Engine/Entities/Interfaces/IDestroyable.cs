namespace Fun.Engine.Entities.Interfaces
{
    public interface IDestroyable
    {
        bool Destroyed { get; }

        void Destroy();
    }
}