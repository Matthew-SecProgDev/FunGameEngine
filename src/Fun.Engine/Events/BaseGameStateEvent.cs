namespace Fun.Engine.Events
{
    /// <summary>
    /// This base class is designed to be inherited by various game events,
    /// allowing you to develop event-specific logic according to your game's requirements.
    /// </summary>
    public abstract class BaseGameStateEvent
    {
        public class Nothing : BaseGameStateEvent;

        public class GameQuit : BaseGameStateEvent;

        public class GameTick : BaseGameStateEvent;
    }
}