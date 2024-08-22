using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fun.Engine.Input
{
    public abstract class BaseInputMapper
    {
        public virtual IEnumerable<BaseInputCommand> GetKeyboardState(GameTime gameTime)
        {
            return new List<BaseInputCommand>();
        }

        public virtual IEnumerable<BaseInputCommand> GetMouseState()
        {
            return new List<BaseInputCommand>();
        }

        // It should be refactored
        public virtual IEnumerable<BaseInputCommand> GetGamePadState(GamePadState state)
        {
            return new List<BaseInputCommand>();
        }
    }
}