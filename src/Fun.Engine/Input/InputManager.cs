using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fun.Engine.Input
{
    public sealed class InputManager(BaseInputMapper inputMapper)
    {
        public void Update()
        {
            Mouse.Instance.Update();
            Keyboard.Instance.Update();
        }

        public void UpdateMouseWorldPosition(Graphics.Screen screen, Graphics.Camera camera)
        {
            var mouse = Mouse.Instance;
            mouse.UpdateWorldPosition(mouse.GetWorldPosition(screen, camera));
        }

        public void GetCommands(GameTime gameTime, Action<BaseInputCommand> handler)
        {
            foreach (var state in inputMapper.GetKeyboardState(gameTime))
            {
                handler(state);
            }

            foreach (var state in inputMapper.GetMouseState())
            {
                handler(state);
            }

            // Currently, it's assumed that only 1 GamePad is used
            var gamePadState = GamePad.GetState(0);
            foreach (var state in inputMapper.GetGamePadState(gamePadState))
            {
                handler(state);
            }
        }
    }
}