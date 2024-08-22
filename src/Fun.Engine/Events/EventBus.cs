namespace Fun.Engine.Events
{
    public class EventBus : Interfaces.IEventBus
    {
        private readonly Dictionary<Type, List<Action<BaseGameStateEvent>>> _handlers = [];

        public void Subscribe<T>(Action<T> handler) where T : BaseGameStateEvent
        {
            var eventType = typeof(T);
            if (!_handlers.ContainsKey(eventType))
            {
                _handlers[eventType] = [];
            }
            _handlers[eventType].Add(e => handler((T)e));
        }

        public void Unsubscribe<T>(Action<T> handler) where T : BaseGameStateEvent
        {
            if (_handlers.TryGetValue(typeof(T), out var handlers))
            {
                handlers.Remove(e => handler((T)e));
            }
        }

        public void Publish<T>(T @event) where T : BaseGameStateEvent
        {
            if (!_handlers.TryGetValue(typeof(T), out var handlers))
            {
                return;
            }

            foreach (var handler in handlers)
            {
                handler(@event);
            }
        }
    }
}