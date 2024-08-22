using System.IO;
using Fun.Engine;
using Fun.Engine.Animations;
using Fun.Engine.Entities;
using Fun.Engine.Entities.Interfaces;
using Fun.Engine.Resources;
using Microsoft.Xna.Framework;

namespace Fun.SimpleGame.Entities
{
    public class PlayerEffect : BaseGameObject, IDimensions
    {
        private float _elapsedTime;

        protected Animation Animation { get; }

        protected Color Color { get; }

        public int Width { get; init; }

        public int Height { get; init; }

        public PlayerEffect(
            IResourceManager resourceManager, 
            string texturePath, 
            string textureName, 
            Color? color = null)
        {
            Texture = resourceManager.LoadTexture(Path.Combine(texturePath, textureName));
            Animation = new Animation(false);
            Color = color ?? Color.White;
        }

        public void AddFrame(int x, int y, int width, int height, float frameTime)
        {
            Animation.AddFrame(new Rectangle(x, y, width, height), frameTime);
        }

        public void SetPosition(int x, int y)
        {
            PositionX = x;
            PositionY = y;
        }

        public void Reset()
        {
            Animation.Reset();
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }

            _elapsedTime += gameTime.GetElapsedSeconds();

            Animation.Update(gameTime);

            if (_elapsedTime > Animation.Lifespan)
            {
                _elapsedTime = 0;
                Deactivate();
            }
        }

        public override void Render(Engine.Graphics.Sprites sprites)
        {
            sprites.Draw(Texture,
                new Rectangle((int)Position.X, (int)Position.Y, Width, Height),
                Animation.CurrentFrame.SourceRectangle,
                Color);
        }

        public void Clear()
        {
            Animation.Clear();
        }
    }
}