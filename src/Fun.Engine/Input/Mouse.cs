using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.Input
{
    public sealed class Mouse
    {
        private static readonly Lazy<Mouse> Lazy = new(() => new Mouse());

        private MouseState _currentState, _previousState;

        public static Mouse Instance => Lazy.Value;

        public Point WindowPosition => _currentState.Position;

        private Vector2 _mouseWorldPosition;
        public void UpdateWorldPosition(Vector2 mousePosition)
        {
            _mouseWorldPosition = mousePosition;
        }
        public Vector2 WorldPosition => _mouseWorldPosition;

        public Mouse()
        {
            _previousState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            _currentState = _previousState;
        }

        public void Update()
        {
            _previousState = _currentState;
            _currentState = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

        public bool IsLeftButtonDown()
        {
            return _currentState.LeftButton == ButtonState.Pressed;
        }

        public bool IsRightButtonDown()
        {
            return _currentState.RightButton == ButtonState.Pressed;
        }

        public bool IsMiddleButtonDown()
        {
            return _currentState.MiddleButton == ButtonState.Pressed;
        }

        public bool IsLeftButtonPressed()
        {
            return _currentState.LeftButton == ButtonState.Pressed &&
                   _previousState.LeftButton == ButtonState.Released;
        }

        public bool IsLeftButtonReleased()
        {
            return _currentState.LeftButton == ButtonState.Released &&
                   _previousState.LeftButton == ButtonState.Pressed;
        }

        public bool IsRightButtonPressed()
        {
            return _currentState.RightButton == ButtonState.Pressed &&
                   _previousState.RightButton == ButtonState.Released;
        }

        public bool IsRightButtonReleased()
        {
            return _currentState.RightButton == ButtonState.Released &&
                   _previousState.RightButton == ButtonState.Pressed;
        }

        public bool IsMiddleButtonPressed()
        {
            return _currentState.MiddleButton == ButtonState.Pressed &&
                   _previousState.MiddleButton == ButtonState.Released;
        }

        public bool IsMiddleButtonReleased()
        {
            return _currentState.MiddleButton == ButtonState.Released &&
                   _previousState.MiddleButton == ButtonState.Pressed;
        }

        public Vector2 GetScreenPosition(Graphics.Screen screen)
        {
#if DEBUG
            ArgumentNullException.ThrowIfNull(screen);
#endif

            var screenDestinationRectangle = screen.CalculateDestinationRectangle();

            var mouseWindowPosition = this.WindowPosition;

            //screen relative position
            float sX = mouseWindowPosition.X - screenDestinationRectangle.X;
            float sY = mouseWindowPosition.Y - screenDestinationRectangle.Y;

            sX /= screenDestinationRectangle.Width;
            sY /= screenDestinationRectangle.Height;

            sX *= screen.Width;
            sY *= screen.Height;

            sY = screen.Height - sY;

            return new Vector2(sX, sY);
        }

        public Vector2 GetWorldPosition(Graphics.Screen screen, Graphics.Camera camera)
        {
#if DEBUG
            ArgumentNullException.ThrowIfNull(screen);
            ArgumentNullException.ThrowIfNull(camera);
#endif

            var screenViewport = new Viewport(0, 0, screen.Width, screen.Height);

            var mouseScreenPosition = this.GetScreenPosition(screen);

            var mouseRay = Mouse.CreateMouseRay(mouseScreenPosition, screenViewport, camera);

            var worldPlane = new Plane(new Vector3(0, 0, 1f), 0f);

            var dist = mouseRay.Intersects(worldPlane);
            var ip = mouseRay.Position + mouseRay.Direction * dist.Value;

            var result = new Vector2(ip.X, ip.Y);
            return result;
        }

        private static Ray CreateMouseRay(Vector2 mouseScreenPosition, Viewport viewport, Graphics.Camera camera)
        {
            var nearPoint = new Vector3(mouseScreenPosition, 0);
            var farPoint = new Vector3(mouseScreenPosition, 1);

            nearPoint = viewport.Unproject(nearPoint, camera.Projection, camera.View, Matrix.Identity);
            farPoint = viewport.Unproject(farPoint, camera.Projection, camera.View, Matrix.Identity);

            var direction = farPoint - nearPoint;
            direction.Normalize();

            var result = new Ray(nearPoint, direction);
            return result;
        }
    }
}