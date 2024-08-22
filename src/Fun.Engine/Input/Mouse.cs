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
            return _currentState.LeftButton == ButtonState.Pressed /*&&
                   _previousState.LeftButton == ButtonState.Pressed*/;
        }

        public bool IsRightButtonDown()
        {
            return _currentState.RightButton == ButtonState.Pressed /*&&
                   _previousState.RightButton == ButtonState.Pressed*/;
        }

        public bool IsMiddleButtonDown()
        {
            return _currentState.MiddleButton == ButtonState.Pressed /*&&
                   _previousState.MiddleButton == ButtonState.Pressed*/;
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

            // Create a viewport based on the game screen.
            var screenViewport = new Viewport(0, 0, screen.Width, screen.Height);

            // Get the mouse pixel coordinates in that screen.
            var mouseScreenPosition = this.GetScreenPosition(screen);

            // Create a ray that starts at the mouse screen position and points "into" the screen towards the game world plane.
            var mouseRay = Mouse.CreateMouseRay(mouseScreenPosition, screenViewport, camera);

            // Plane where the 2D game world takes place.
            var worldPlane = new Plane(new Vector3(0, 0, 1f), 0f);

            // Determine the point where the ray intersects the game world plane.
            var dist = mouseRay.Intersects(worldPlane);
            var ip = mouseRay.Position + mouseRay.Direction * dist.Value;

            // Send the result as a 2D world position vector.
            var result = new Vector2(ip.X, ip.Y);
            return result;
        }

        private static Ray CreateMouseRay(Vector2 mouseScreenPosition, Viewport viewport, Graphics.Camera camera)
        {
            // Near and far points that will indicate the line segment used to define the ray.
            var nearPoint = new Vector3(mouseScreenPosition, 0);
            var farPoint = new Vector3(mouseScreenPosition, 1);

            // Convert the near and far points to world coordinates.
            nearPoint = viewport.Unproject(nearPoint, camera.Projection, camera.View, Matrix.Identity);
            farPoint = viewport.Unproject(farPoint, camera.Projection, camera.View, Matrix.Identity);

            // Determine the direction.
            var direction = farPoint - nearPoint;
            direction.Normalize();

            // Resulting ray starts at the near mouse position and points "into" the screen.
            var result = new Ray(nearPoint, direction);
            return result;
        }
    }
}