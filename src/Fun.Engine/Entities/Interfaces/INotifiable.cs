namespace Fun.Engine.Entities.Interfaces
{
    public interface INotifiable
    {
        void OnNotify(Events.BaseGameStateEvent @event);
    }
}