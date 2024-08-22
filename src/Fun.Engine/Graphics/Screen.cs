using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.Engine.Graphics
{
    public sealed class Screen : IDisposable
    {
        private readonly Game _game;
        private readonly RenderTarget2D _renderTarget;

        private bool _isDisposed;
        private bool _isSet;

        public const int MinDimension = 64;
        public const int MaxDimension = 4096;

        public int Width => _renderTarget.Width;

        public int Height => _renderTarget.Height;

        public Screen(Game game, int width, int height)
        {
            ArgumentNullException.ThrowIfNull(game);

            _game = game;

            width = FunMath.Clamp(width, MinDimension, MaxDimension);
            height = FunMath.Clamp(height, MinDimension, MaxDimension);

            _renderTarget = new RenderTarget2D(_game.GraphicsDevice, 
                width, 
                height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                0,
                RenderTargetUsage.DiscardContents);

            _isDisposed = false;
            _isSet = false;
        }

        public void Set()
        {
#if DEBUG
            if (_isSet)
            {
                const string msg = "Render target is already set.";
                throw new Exception(msg);
            }
#endif

            // Set the render target
            _game.GraphicsDevice.SetRenderTarget(_renderTarget);
            _game.GraphicsDevice.Clear(Color.CornflowerBlue);

            _isSet = true;
        }

        public void UnSet()
        {
#if DEBUG
            if (!_isSet)
            {
                const string msg = "Render target is not set.";
                throw new Exception(msg);
            }
#endif

            // Reset the render target to the back buffer
            _game.GraphicsDevice.SetRenderTarget(null);

            _isSet = false;
        }

        public void Present([NotNull]Sprites? sprites, bool textureFiltering = true)
        {
#if DEBUG
            if (_isSet)
            {
                throw new Exception("The \"Screen\" is currently set as the render target. " +
                                    "\"UnSet\" the \"Screen\" before presenting.");
            }

            ArgumentNullException.ThrowIfNull(sprites);

            _game.GraphicsDevice.Clear(ClearOptions.Target, Color.HotPink, 1.0f, 0);
#else
            _game.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);
#endif

            sprites.BeginImmediately(textureFiltering);
            sprites.Draw(_renderTarget, this.CalculateDestinationRectangle(), null, Color.White);
            sprites.End();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal Rectangle CalculateDestinationRectangle()
        {
            var backBufferBounds = _game.GraphicsDevice.PresentationParameters.Bounds;
            var backBufferAspectRatio = (float)backBufferBounds.Width / backBufferBounds.Height;
            var screenAspectRatio = (float)Width / Height;

            var rectPosX = 0f;
            var rectPosY = 0f;
            var rectWidth = (float)backBufferBounds.Width;
            var rectHeight = (float)backBufferBounds.Height;

            if (backBufferAspectRatio > screenAspectRatio)
            {
                rectWidth = rectHeight * screenAspectRatio;
                rectPosX = (backBufferBounds.Width - rectWidth) / 2f;
            }
            else if (backBufferAspectRatio < screenAspectRatio)
            {
                rectHeight = rectWidth / screenAspectRatio;
                rectPosY = (backBufferBounds.Height - rectHeight) / 2f;
            }

            return new Rectangle((int)rectPosX, (int)rectPosY, (int)rectWidth, (int)rectHeight);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    _renderTarget.Dispose();
                }

                // TODO: Override finalizer (destructor) only if there is code to free unmanaged resources
                // Free unmanaged resources (unmanaged objects)
                // Set large fields to null

                _isDisposed = true;
            }
        }
    }
}