namespace Fun.Engine.Events
{
    public class EventBus : Interfaces.IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = new();

        public void Subscribe<T>(Action<T> handler) where T : BaseGameStateEvent
        {
            ArgumentNullException.ThrowIfNull(handler);

            var eventType = typeof(T);

            if (!_handlers.TryGetValue(eventType, out var handlers))
            {
                handlers = new List<Delegate>();
                _handlers.Add(eventType, handlers);
            }

            handlers.Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler) where T : BaseGameStateEvent
        {
            if (!_handlers.TryGetValue(typeof(T), out var handlers))
            {
                return;
            }

            handlers.Remove(handler);

            if (handlers.Count == 0)
            {
                _handlers.Remove(typeof(T));
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
                ((Action<T>)handler)(@event);
            }
        }
    }
}