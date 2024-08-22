namespace Fun.Engine.Entities.Interfaces
{
    public interface IDestroyable
    {
        bool Destroyed { get; }

        /// <summary>
        /// This function marks an object within the game state as destroyed, ceasing all operations on it.
        /// When an object is marked as destroyed, it signals the need to remove it from the game state.
        /// Implement game state logic to call the RemoveGameObject function to effectively remove any destroyed objects.
        /// </summary>
        void Destroy();
    }
}