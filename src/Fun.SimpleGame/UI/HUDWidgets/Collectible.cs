using Microsoft.Xna.Framework;
using System;
using Fun.Engine;
using Fun.Engine.Entities.Interfaces;
using Fun.Engine.Physics;
using Fun.Engine.Resources;

namespace Fun.SimpleGame.UI.HUDWidgets
{
    public class Collectible : BaseGameCollidableObject, IDimensions
    {
        private float _shakeTime;
        private float _originalPosY;

        public int Amount { get; init; }

        public AssetType Type { get; init; }

        public int Width { get; init; }

        public int Height { get; init; }

        public Collectible(IResourceManager resourceManager, string assetName)
        {
            Texture = resourceManager.LoadTexture(assetName);
        }

        public override void Initialize()
        {
            _originalPosY = Position.Y;
        }

        public override void OnNotify(Engine.Events.BaseGameStateEvent @event)
        {
            if (@event is States.Gameplay.GameplayEvents.ScreenPositionChanged screenEvent)
            {
                ScreenPosition = screenEvent.Position;
            }
        }

        public void Remove()
        {
            //this approach is not good it should be refactored
            RemoveObject(this);
        }

        public void Update(GameTime gameTime)
        {
            _shakeTime += gameTime.GetElapsedSeconds();
            var shakeOffsetY = MathF.Cos(_shakeTime * 10f) * 4f;
            PositionY = _originalPosY + shakeOffsetY;
        }

        public override void Render(Engine.Graphics.Sprites sprites)
        {
            var x = (int)ScreenPosition.X + (int)Position.X;
            var y = (int)ScreenPosition.Y + (int)Position.Y;
            var destRectangle = new Rectangle(x, y, Width, Height);

            sprites.Draw(Texture, destRectangle, null, Color.White);
        }
    }
}