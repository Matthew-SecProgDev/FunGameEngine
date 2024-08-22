using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework;

namespace Fun.Engine
{
    /// <summary>
    /// This class is developed as a main point for the game.
    /// </summary>
    public class MainGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly int _designedResolutionWidth;
        private readonly int _designedResolutionHeight;
        private readonly IServiceProvider _serviceProvider;

        private States.BaseGameState _currentGameState;
        private Graphics.Screen _screen;
        private Graphics.Sprites _sprites;
        private Graphics.Shapes _shapes;
        private Graphics.Camera _camera;
        private Graphics.ScreenShake _screenShake;

        public MainGame(IOptions<Configurations.MainConfig> configuration, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = true,
                IsFullScreen = configuration.Value.IsFullScreen
            };

            _designedResolutionWidth = configuration.Value.Width;
            _designedResolutionHeight = configuration.Value.Height;

            Content.RootDirectory = configuration.Value.RootDirectory;
            IsMouseVisible = configuration.Value.IsMouseVisible;
            IsFixedTimeStep = configuration.Value.IsFixedTimeStep;

            var value = TimeSpan.TicksPerSecond / (double)configuration.Value.UpdatesCountPerSecond;
            TargetElapsedTime = TimeSpan.FromTicks((long)Math.Round(value));
        }

        public void SetCurrentGameState(States.BaseGameState firstGameState)
        {
            _currentGameState = firstGameState;
        }

        /// <summary>
        /// Initializes graphics settings and rendering instances.
        /// </summary>
        protected override void Initialize()
        {
            if (_currentGameState == null)
            {
                throw new Exception("");
            }

            //Util.SetRelateBackBufferSize(_graphics, 0.85f)
            var displayMode = GraphicsDevice.DisplayMode;
            _graphics.PreferredBackBufferWidth = (int)(displayMode.Width * 0.85f);
            _graphics.PreferredBackBufferHeight = (int)(displayMode.Height * 0.85f);
            _graphics.ApplyChanges();

            _screen = new Graphics.Screen(this, _designedResolutionWidth, _designedResolutionHeight);
            _sprites = new Graphics.Sprites(this);
            //_shapes = new Graphics.Shapes(this);
            _shapes = new Graphics.Shapes(_sprites);
            _screenShake = new Graphics.ScreenShake();
            _camera = new Graphics.Camera(_screen, _screenShake);
            //_camera.Zoom = 5;

            //GraphicsDevice.DepthStencilState = new DepthStencilState
            //{
            //    DepthBufferEnable = true
            //};

            base.Initialize();
        }

        /// <summary>
        /// LoadContent is called once per game to load all content.
        /// </summary>
        protected override void LoadContent()
        {
            this.SwitchGameState(_currentGameState);
        }

        /// <summary>
        /// UnloadContent is called to release all resources of the game.
        /// </summary>
        protected override void UnloadContent()
        {
            _currentGameState.UnloadContent();
            _shapes.Dispose();
            _sprites.Dispose();
            _screen.Dispose();

            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            _currentGameState.HandleInput(gameTime);
            _currentGameState.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            // Render to the Render Target
            _screen.Set();
            //_shapes.Begin(_camera);//must be put after _sprites
            _sprites.Begin(_camera, true); //maybe false
            
            _currentGameState.Render(_sprites);

            _sprites.End();
            //_shapes.End();//must be put after _sprites

            //_shapes.Begin(_camera);
            //_shapes.DrawLine(new Vector2(10, 10), new Vector2(100, 100), 5f, Color.Red);
            //_shapes.End();

            // Now render the scaled content
            _screen.UnSet();
            _screen.Present(_sprites);

            base.Draw(gameTime);
        }

        /// <summary>
        /// SwitchGameState is used to transition between game states. It sets and unsets event handlers,
        /// and calls UnloadContent, Initialize, and LoadContent for the current state.
        /// </summary>
        /// <param name="gameState"></param>
        private void SwitchGameState(States.BaseGameState gameState)
        {
            _currentGameState.OnStateSwitched -= this.CurrentGameState_OnStateSwitched;
            _currentGameState.OnEventNotification -= this.CurrentGameState_OnEventNotification;

            //is it correct that any time call UnloadContent?
            //I think it's correct just when a level is done and the user want to move to another level,
            //and it's not call for where that probably run many times for example for run menu states and so on so
            //all resources for menu states and other states like that must be remained in ResourceManager 
            _currentGameState.UnloadContent();

            _currentGameState = gameState;

            //refactor it
            var viewport = _graphics.GraphicsDevice.Viewport;
            _currentGameState.Initialize(Content, _camera, _screen, _shapes, viewport.Width, viewport.Height);

            _currentGameState.LoadContent();

            _currentGameState.OnStateSwitched += this.CurrentGameState_OnStateSwitched;
            _currentGameState.OnEventNotification += this.CurrentGameState_OnEventNotification;
        }

        /// <summary>
        /// This function acts as a callback for the OnEventNotification event handler.
        /// It is triggered to notify the main game engine of an event within a game state,
        /// prompting the engine to perform specific actions based on that event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentGameState_OnEventNotification(object? sender, Events.BaseGameStateEvent e)
        {
            switch (e)
            {
                case Fun.Engine.Events.BaseGameStateEvent.GameQuit:
                    Exit();
                    break;
            }
        }

        /// <summary>
        /// This function serves as a callback for the OnStateSwitched handler.
        /// It is triggered by calling the SwitchState function within a game state to switch to a specified game state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentGameState_OnStateSwitched(object? sender, States.BaseGameState e)
        {
            SwitchGameState(e);
        }
    }
}