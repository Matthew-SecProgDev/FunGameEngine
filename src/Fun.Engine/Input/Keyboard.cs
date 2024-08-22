using Microsoft.Xna.Framework.Input;

namespace Fun.Engine.Input
{
    public sealed class Keyboard
    {
        private static readonly Lazy<Keyboard> Lazy = new(() => new Keyboard());

        private KeyboardState _currentKeyboardState, _previousKeyboardState;

        public static Keyboard Instance => Lazy.Value;

        public bool IsCapsLockActive => _currentKeyboardState.CapsLock;

        public bool IsKeyAvailable => _currentKeyboardState.GetPressedKeyCount() > 0;

        public Keyboard()
        {
            _previousKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            _currentKeyboardState = _previousKeyboardState;
        }

        public void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        }

        public bool IsKeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) &&
                   _previousKeyboardState.IsKeyUp(key);
        }

        public Keys[] GetPressedKeys()
        {
            return _currentKeyboardState.GetPressedKeys();
        }
    }
}