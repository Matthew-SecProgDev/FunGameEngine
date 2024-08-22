namespace Fun.Engine.Events.Interfaces
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> handler) where T : BaseGameStateEvent;

        void Unsubscribe<T>(Action<T> handler) where T : BaseGameStateEvent;

        void Publish<T>(T @event) where T : BaseGameStateEvent;
    }
}