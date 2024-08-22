using System.Diagnostics.CodeAnalysis;
using Fun.Engine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Fun.Engine.States
{
    /// <summary>
    /// This base class is designed for inheriting by various game states.
    /// It provides common functionalities and properties that all game states should have.
    /// </summary>
    public abstract class BaseGameState
    {
        protected bool IsDebugCollisionEnabled { get; set; } = false;

        protected bool _indestructible = false;//it should be refactored

        protected bool IsFourViewsEnabled { get; set; } = false;

        private readonly List<Entities.BaseObject> _objects = [];

        protected Input.InputManager InputManager { get; }

        protected int ViewportHeight { get; private set; }

        protected int ViewportWidth { get; private set; }

        protected IResourceManager ResourceManager { get; private set; }

        protected Graphics.Camera Camera { get; private set; }

        protected Graphics.Screen Screen { get; private set; }

        protected Graphics.Shapes Shapes { get; private set; }

        /// <summary>
        /// Initializes the InputManager with a provided inputMapper and sets up the SoundManager for a game state.
        /// </summary>
        /// <param name="inputMapper"></param>
        protected BaseGameState(Input.BaseInputMapper inputMapper)
        {
            this.InputManager = new Input.InputManager(inputMapper);
        }

        /// <summary>
        /// Initializes ResourceManager, Camera and sets viewport dimensions.
        /// Call this method when switching between game states.
        /// </summary>
        /// <param name="contentManager"></param>
        /// <param name="camera"></param>
        /// <param name="screen"></param>
        /// <param name="shapes"></param>
        /// <param name="viewportWidth"></param>
        /// <param name="viewportHeight"></param>
        internal void Initialize(
            ContentManager contentManager,
            Graphics.Camera camera,
            Graphics.Screen screen,
            Graphics.Shapes shapes, 
            int viewportWidth,
            int viewportHeight)
        {
            // this function should be refactored and use another way to do these works
            this.ResourceManager = new ResourceManager(contentManager);
            this.Camera = camera;
            this.Screen = screen;
            this.Shapes = shapes;
            this.ViewportHeight = viewportHeight;
            this.ViewportWidth = viewportWidth;
        }

        /// <summary>
        /// LoadContent will be called once per each game state and is the place to load all of your content.
        /// </summary>
        public abstract void LoadContent();

        /// <summary>
        /// Event handler designated for use within the game engine only.
        /// Do not set or use in game development.
        /// </summary>
        public event EventHandler<BaseGameState>? OnStateSwitched;
        /// <summary>
        /// Switches to the specified game state.
        /// </summary>
        /// <param name="gameState"></param>
        protected void SwitchState(BaseGameState gameState)
        {
            this.OnStateSwitched?.Invoke(this, gameState);
        }

        /// <summary>
        /// Event handler designated for use within the game engine only.
        /// Do not set or use in game development.
        /// </summary>
        public event EventHandler<Events.BaseGameStateEvent>? OnEventNotification;
        /// <summary>
        /// Notifies all objects in the game state of an event and plays the associated sound if set.
        /// </summary>
        /// <param name="event"></param>
        protected void NotifyEvent(Events.BaseGameStateEvent @event)
        {
            this.OnEventNotification?.Invoke(this, @event);

            for (var i = 0; i < _objects.Count; i++)
            {
                _objects[i]?.OnNotify(@event);
            }
        }

        /// <summary>
        /// Checks incoming inputs in game states.
        /// Avoid using this function for collision checks or similar operations.
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void HandleInput(GameTime gameTime);

        /// <summary>
        /// Should be called only in the game engine, not from within any game state.
        /// This method triggers the UpdateState function for a specified game state.
        /// </summary>
        /// <param name="gameTime"></param>
        internal void Update(GameTime gameTime)
        {
            this.UpdateState(gameTime);
        }

        /// <summary>
        /// Allows the game state to run logic such as updating the world, checking for collisions.
        /// </summary>
        /// <param name="gameTime"></param>
        protected abstract void UpdateState(GameTime gameTime);

        /// <summary>
        /// Releases all resources occupied by the game state's ResourceManager.
        /// </summary>
        public virtual void UnloadContent()
        {
            this.ResourceManager?.Unload();
        }

        /// <summary>
        /// Adds an object to the game state’s private list for rendering on the screen and receiving events.
        /// </summary>
        /// <param name="object"></param>
        protected void AddObject(Entities.BaseObject @object)
        {
            @object.Initialize();

            _objects.Add(@object);
        }

        /// <summary>
        /// Removes an object from the game state’s private list when it's no longer needed,
        /// such as during a game reset while remaining in the same state.
        /// </summary>
        /// <param name="object"></param>
        protected void RemoveObject(Entities.BaseObject @object)
        {
            _objects.Remove(@object);
        }

        /// <summary>
        /// Renders all objects in the game state’s private list, drawing them on the screen.
        /// </summary>
        /// <param name="sprites"></param>
        public virtual void Render([NotNull] Graphics.Sprites? sprites)
        {
#if DEBUG
            ArgumentNullException.ThrowIfNull(sprites);
#endif

            // it should be refactored
            //_objects.OrderBy(i=>i.ZIndex).f
            for (var i = 0; i < _objects.Count; i++)
            {
                //var @object = _objects[i];
                if (_objects[i] is { IsActive: true })
                {
                    if (IsDebugCollisionEnabled && _objects[i] is Physics.BaseGameCollidableObject collidableObject)
                    {
                        collidableObject.RenderCollisionBox(sprites);
                    }

                    _objects[i].Render(sprites);
                }
            }
        }
    }
}