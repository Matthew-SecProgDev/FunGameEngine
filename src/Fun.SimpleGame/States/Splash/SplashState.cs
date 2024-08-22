using Fun.Engine.States;
using Fun.SimpleGame.Entities;
using Fun.SimpleGame.States.MainMenu;
using Microsoft.Xna.Framework;

namespace Fun.SimpleGame.States.Splash
{
    public class SplashState() : BaseGameState(new SplashInputMapper())
    {
        public override void LoadContent()
        {
            AddObject(new SplashSprite(ResourceManager));
        }

        public override void HandleInput(GameTime gameTime)
        {
            InputManager.GetCommands(gameTime, cmd =>
            {
                if (cmd is SplashInputCommand.GameSelect)
                {
                    SwitchState(new MainMenuState());
                }
            });
        }

        protected override void UpdateState(GameTime gameTime)
        {
            // Ignored
        }
    }
}