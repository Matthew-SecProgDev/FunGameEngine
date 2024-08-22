using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.Engine.Graphics
{
    public sealed class Camera
    {
        private Vector2 _position;
        private float _baseZ;
        private float _z;

        private float _aspectRatio;
        private float _fieldOfView;

        private Matrix _view;
        private Matrix _projection;

        private int _zoom;

        public const float MinZ = 1f;
        public const float MaxZ = 2048f;
        public const int MinZoom = 1;
        public const int MaxZoom = 20;

        public Vector2 Position => _position;

        public float Z => _z;

        public float BaseZ => _baseZ;

        public Matrix View => _view;

        public Matrix Projection => _projection;

        public ScreenShake _screenShake;
        public Screen _screen;

        public Vector2 MouseScreenPosition => Input.Mouse.Instance.GetScreenPosition(_screen);

        public Camera(Screen screen, ScreenShake screenShake)
        {
            ArgumentNullException.ThrowIfNull(screen);
            ArgumentNullException.ThrowIfNull(screenShake);

            _aspectRatio = (float)screen.Width / screen.Height;
            _fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver2;

            _position = Vector2.Zero;
            _baseZ = this.GetZFromHeight(screen.Height);
            _z = _baseZ;

            this.UpdateMatrices();

            _zoom = 1;

            _screenShake = screenShake;
            _screen = screen;
        }

        public void UpdateMatrices()
        {
            _view = Matrix.CreateLookAt(new Vector3(0f, 0f, _z), Vector3.Zero, Vector3.Up);
            _projection = Matrix.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, MinZ, MaxZ);
        }

        public void ApplyShake(float duration, float magnitude)
        {
            _screenShake.StartShake(duration, magnitude);
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            throw new NotImplementedException();
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            throw new NotImplementedException();
        }

        public float GetZFromHeight(float height)
        {
            return (0.5f * height) / MathF.Tan(0.5f * _fieldOfView);
        }

        public float GetHeightFromZ()
        {
            return _z * MathF.Tan(0.5f * _fieldOfView) * 2f;
        }

        public void MoveZ(float amount)
        {
            _z += amount;
            _z = FunMath.Clamp(_z, MinZ, MaxZ);
        }

        public void ResetZ()
        {
            _z = _baseZ;
        }

        public void Move(Vector2 amount)
        {
            _position += amount;
        }

        public void MoveTo(Vector2 position)
        {
            _position = position;
        }

        public void IncreaseZoom()
        {
            _zoom++;
            _zoom = FunMath.Clamp(_zoom, MinZoom, MaxZoom);
            _z = _baseZ / _zoom;
        }

        public void DecreaseZoom()
        {
            _zoom--;
            _zoom = FunMath.Clamp(_zoom, MinZoom, MaxZoom);
            _z = _baseZ / _zoom;
        }

        public void SetZoom(int amount)
        {
            _zoom = amount;
            _zoom = FunMath.Clamp(_zoom, MinZoom, MaxZoom);
            _z = _baseZ / _zoom;
        }

        public void GetExtents(out float width, out float height)
        {
            height = this.GetHeightFromZ();
            width = height * _aspectRatio;
        }

        public void GetExtents(out float left, out float right, out float bottom, out float top)
        {
            this.GetExtents(out float width, out float height);

            left = _position.X - width * 0.5f;
            right = left + width;
            bottom = _position.Y - height * 0.5f;
            top = bottom + height;
        }

        public void GetExtents(out Vector2 min, out Vector2 max)
        {
            this.GetExtents(out float left, out float right, out float bottom, out float top);

            min = new Vector2(left, bottom);
            max = new Vector2(right, top);
        }
    }
}