using System.Collections.Generic;
using Fun.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fun.SimpleGame.States.Splash
{
    public class SplashInputMapper : BaseInputMapper
    {
        public override IEnumerable<BaseInputCommand> GetKeyboardState(GameTime gameTime)
        {
            var commands = new List<SplashInputCommand>();
            var keyboard = Fun.Engine.Input.Keyboard.Instance;

            if (keyboard.IsKeyDown(Keys.Enter))
            {
                commands.Add(new SplashInputCommand.GameSelect());
            }

            return commands;
        }
    }
}