using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.Graphics
{
    public sealed class Sprites : IDisposable
    {
        private readonly Game _game;
        private readonly SpriteBatch _sprites;
        private readonly BasicEffect _effect;

        private bool _isDisposed;

        public GraphicsDevice GraphicsDevice => _sprites.GraphicsDevice;

        public Sprites(Game game)
        {
            ArgumentNullException.ThrowIfNull(game);

            _game = game;
            _sprites = new SpriteBatch(game.GraphicsDevice);
            _effect = new BasicEffect(game.GraphicsDevice)
            {
                FogEnabled = false,
                LightingEnabled = false,
                VertexColorEnabled = true,
                PreferPerPixelLighting = false,//TODO: What does it do? adjusting false is correct?
                Texture = null,//TODO: What does it do? adjusting null is correct?
                TextureEnabled = true,
                World = Matrix.Identity,
                Projection = Matrix.Identity,
                View = Matrix.Identity
            };

            _isDisposed = false;
        }

        public void Begin(Camera? camera, bool textureFiltering)
        {
            var sampler = SamplerState.PointClamp;
            if (textureFiltering)
            {
                sampler = SamplerState.LinearClamp;
            }

            if (camera is null)
            {
                var viewport = _game.GraphicsDevice.Viewport;
                _effect.Projection = Matrix.CreateOrthographicOffCenter(0f, 
                    viewport.Width, 
                    0f, 
                    viewport.Height, 
                    0f, 
                    1f);

                _effect.View = Matrix.Identity;
            }
            else
            {
                camera.UpdateMatrices();

                _effect.Projection = camera.Projection;
                _effect.View = camera.View * Matrix.CreateTranslation(camera._screenShake.ShakeOffset.X, camera._screenShake.ShakeOffset.Y, 0f);
                //_effect.View = camera.View;
                //* Matrix.CreateTranslation(screenShake.ShakeOffset.X, screenShake.ShakeOffset.Y, 0f);

                // TODO: Do I really want anisotropic filtering whenever the camera is farther away then the base Z.
                if (camera.Z > camera.BaseZ)
                {
                    sampler = SamplerState.AnisotropicClamp;
                }
            }

            //var blend = new BlendState();
            //blend.AlphaSourceBlend = Blend.Zero;
            //blend.AlphaDestinationBlend = Blend.InverseSourceColor;
            //blend.ColorSourceBlend = Blend.Zero;
            //blend.ColorDestinationBlend = Blend.InverseSourceColor;

            //_sprites.Begin(
            //    blendState: blend,
            //    samplerState: sampler,
            //    rasterizerState: RasterizerState.CullNone,
            //    effect: _effect);

            _sprites.Begin(
                blendState: BlendState.AlphaBlend,
                samplerState: sampler,
                rasterizerState: RasterizerState.CullNone,
                effect: _effect);
        }

        internal void BeginImmediately(bool isTextureFilteringEnabled)
        {
            var sampler = SamplerState.PointClamp;
            if (isTextureFilteringEnabled)
            {
                sampler = SamplerState.LinearClamp;
            }

            var viewport = _game.GraphicsDevice.Viewport;
            _effect.Projection = Matrix.CreateOrthographicOffCenter(0f,
                viewport.Width,
                0f,
                viewport.Height,
                0f,
                1f);

            _effect.View = Matrix.Identity;

            _sprites.Begin(
                sortMode: SpriteSortMode.Immediate,
                blendState: BlendState.AlphaBlend,
                samplerState: sampler,
                rasterizerState: RasterizerState.CullNone,
                effect: _effect);
        }

        public void End()
        {
            _sprites.End();
        }

        public void Draw(Texture2D texture,
            Vector2 position,
            Rectangle? sourceRectangle,
            Color color)
        {
            _sprites.Draw(texture,
                position,
                sourceRectangle,
                color,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.FlipVertically,
                0f);
        }

        public void Draw(Texture2D texture,
            Rectangle destinationRectangle,
            Rectangle? sourceRectangle,
            Color color)
        {
            _sprites.Draw(texture,
                destinationRectangle,
                sourceRectangle,
                color,
                0f,
                Vector2.Zero,
                SpriteEffects.FlipVertically,
                0f);
        }

        public void Draw(Texture2D texture, 
            Vector2 position, 
            Rectangle? sourceRectangle, 
            Color color, 
            SpriteEffects effect)
        {
            _sprites.Draw(texture,
                position,
                sourceRectangle,
                color,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.FlipVertically | effect,
                0f);
        }

        public void Draw(Texture2D texture,
            Rectangle destinationRectangle,
            Rectangle? sourceRectangle,
            Color color,
            SpriteEffects effect)
        {
            _sprites.Draw(texture,
                destinationRectangle,
                sourceRectangle,
                color,
                0f,
                Vector2.Zero,
                SpriteEffects.FlipVertically | effect,
                0f);
        }

        public void Draw(Texture2D texture,
            //[NotNull]Texture2D? texture,
            Vector2 position,
            Rectangle? sourceRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effect,
            float layerDepth)
        {
//#if DEBUG
//            ArgumentNullException.ThrowIfNull(texture);
//#endif

            _sprites.Draw(texture,
                position,
                sourceRectangle,
                color,
                rotation,
                origin,
                scale,
                SpriteEffects.FlipVertically | effect,
                layerDepth);
        }

        public void Draw(Texture2D texture,
            Vector2 position,
            Rectangle? sourceRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effect,
            float layerDepth)
        {
            _sprites.Draw(texture,
                position,
                sourceRectangle,
                color,
                rotation,
                origin,
                scale,
                SpriteEffects.FlipVertically | effect,
                layerDepth);
        }

        public void Draw(Texture2D texture,
            Vector2 position,
            Rectangle? sourceRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            float layerDepth)
        {
            _sprites.Draw(texture,
                position,
                sourceRectangle,
                color,
                rotation,
                origin,
                scale,
                SpriteEffects.FlipVertically,
                layerDepth);
        }

        public void Draw(Texture2D texture,
            Vector2 position,
            Rectangle? sourceRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            float scale,
            float layerDepth)
        {
            _sprites.Draw(texture,
                position,
                sourceRectangle,
                color,
                rotation,
                origin,
                scale,
                SpriteEffects.FlipVertically,
                layerDepth);
        }

        public void DrawString(SpriteFont font, string text, Vector2 position, Color color)
        {
            _sprites.DrawString(font,
                text,
                position,
                color,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.FlipVertically,
                0f);
        }

        public void DrawString(SpriteFont font, StringBuilder text, Vector2 position, Color color)
        {
            _sprites.DrawString(font,
                text,
                position,
                color,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.FlipVertically,
                0f);
        }

        public void DrawString(
            SpriteFont font,
            string text,
            Vector2 position,
            Color color,
            Vector2 origin,
            SpriteEffects effect,
            float rotation = 0f,
            float scale = 1f,
            float layerDepth = 0f)
        {
            _sprites.DrawString(font,
                text,
                position,
                color,
                rotation,
                origin,
                scale,
                SpriteEffects.FlipVertically | effect,
                layerDepth);
        }

        public void DrawString(SpriteFont font,
            StringBuilder text,
            Vector2 position,
            Color color,
            Vector2 origin,
            SpriteEffects effect,
            float rotation = 0f,
            float scale = 1f,
            float layerDepth = 0f)
        {
            _sprites.DrawString(font,
                text,
                position,
                color,
                rotation,
                origin,
                scale,
                SpriteEffects.FlipVertically | effect,
                layerDepth);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    _effect.Dispose();
                    _sprites.Dispose();
                }

                // TODO: Override finalizer (destructor) only if there is code to free unmanaged resources
                // Free unmanaged resources (unmanaged objects)
                // Set large fields to null

                _isDisposed = true;
            }
        }
    }
}