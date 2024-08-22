using System.Collections.Generic;
using Fun.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fun.SimpleGame.States.MainMenu
{
    public class MainMenuInputMapper : BaseInputMapper
    {
        public override IEnumerable<BaseInputCommand> GetKeyboardState(GameTime gameTime)
        {
            var commands = new List<MainMenuInputCommand>();
            var keyboard = Fun.Engine.Input.Keyboard.Instance;

            if (keyboard.IsKeyDown(Keys.Escape))
            {
                commands.Add(new MainMenuInputCommand.MenuExit());
            }

            return commands;
        }
    }
}