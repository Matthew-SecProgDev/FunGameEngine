namespace Fun.Engine.Events.Interfaces
{
    public interface IEventHandler<in TEvent> where TEvent : BaseGameStateEvent
    {
        void Handle(TEvent @event);
    }
}