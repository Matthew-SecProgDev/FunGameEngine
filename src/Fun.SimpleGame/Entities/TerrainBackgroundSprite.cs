using System;
using Fun.Engine.Entities;
using Fun.Engine.Events.Interfaces;
using Fun.Engine.Graphics;
using Fun.Engine.Resources;
using Fun.SimpleGame.Entities.Enums;
using Fun.SimpleGame.States.Gameplay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.SimpleGame.Entities
{
    public class TerrainBackgroundSprite : BaseObject
    {
        private const float ScrollingSpeed = 160f;

        private readonly Texture2D _texture;
        private readonly IEventBus _eventBus;
        private readonly Camera _camera;

        private int _textureIndex = 1;
        private const int _offsetUpHeight = 5;//shake screen
        private const int _offsetDownHeight = 10;//shake screen

        public TerrainBackgroundSprite(IResourceManager resourceManager, IEventBus eventBus, Camera camera)
        {
            const string texturePath = "Textures/Terrain/terrain_texture";
            _texture = resourceManager.LoadTexture(texturePath);
            _eventBus = eventBus;
            _camera = camera;
        }

        public override void Initialize()
        {
            PositionY = -_camera.GetHeightFromZ() * 0.5f - _offsetUpHeight;
        }

        public void HandleMoving(GameplayEvents.GoingToLocation location)
        {
            switch (location.Direction)
            {
                case DirectionType.Right:
                    HandleMoveRight(location.CurrentPosition, location.ElapsedTime);
                    break;
                case DirectionType.Left:
                    HandleMoveLeft(location.CurrentPosition, location.ElapsedTime);
                    break;
                case DirectionType.Up:
                case DirectionType.Down:
                    HandleVerticalMove(location);
                    break;
                default:
                    throw new Exception("Invalid operation");
            }
        }

        public override void Render(Engine.Graphics.Sprites sprites)
        {
            var viewport = sprites.GraphicsDevice.Viewport;

            if (Position.X + viewport.Width * _textureIndex < 1)
            {
                _textureIndex++;
            }

            DrawTexture(sprites, _textureIndex - 2);
            DrawTexture(sprites, _textureIndex - 1);
            DrawTexture(sprites, _textureIndex);
        }

        private void DrawTexture(Engine.Graphics.Sprites sprites, int indexOffset)
        {
            _camera.GetExtents(out float width, out float height);
            var widthInt = (int)width;
            var x = (int)Position.X + widthInt * indexOffset;
            var rect = new Rectangle(x, (int)Position.Y, widthInt, (int)height + _offsetDownHeight);
            sprites.Draw(_texture, rect, null, Color.White);
        }

        private void HandleMoveRight(Vector2 characterPosition, float elapsedTime)
        {
            var positionX = KeepXWithinBounds(Position.X - ScrollingSpeed * elapsedTime);
            PositionX = positionX;
            _eventBus.Publish(new GameplayEvents.ScreenPositionChanged(new Vector2(Position.X, 0f)));
            _eventBus.Publish(new GameplayEvents.PlayerMoveRight
            {
                PosX = KeepXWithinBounds(characterPosition.X),
                PosY = KeepYWithinBounds(characterPosition.Y)
            });
        }

        private void HandleMoveLeft(Vector2 characterPosition, float elapsedTime)
        {
            var positionX =
                KeepXWithinBounds(Position.X +
                                  ScrollingSpeed * elapsedTime);
            PositionX = positionX;
            _eventBus.Publish(new GameplayEvents.ScreenPositionChanged(new Vector2(Position.X, 0f)));
            _eventBus.Publish(new GameplayEvents.PlayerMoveLeft
            {
                PosX = KeepXWithinBounds(characterPosition.X),
                PosY = KeepYWithinBounds(characterPosition.Y)
            });
        }

        private void HandleVerticalMove(GameplayEvents.GoingToLocation location)
        {
            if (!location.DestinationPosition.HasValue)
            {
                throw new Exception("Invalid location: Destination position is missing.");
            }

            if (CheckValidateMoveOn(location.CurrentPosition, location.DestinationPosition.Value, location.Direction))
            {
                // probably here there is a mechanism to send events to a server or anti-cheat system
            }

            if (location.Direction == DirectionType.Up)
            {
                _eventBus.Publish(new GameplayEvents.PlayerMoveUp
                {
                    PosX = KeepXWithinBounds(location.DestinationPosition.Value.X),
                    PosY = KeepYWithinBounds(location.DestinationPosition.Value.Y)
                });
            }
            else
            {
                _eventBus.Publish(new GameplayEvents.PlayerMoveDown
                {
                    PosX = KeepXWithinBounds(location.DestinationPosition.Value.X),
                    PosY = KeepYWithinBounds(location.DestinationPosition.Value.Y)
                });
            }

            _eventBus.Publish(new GameplayEvents.ScreenPositionChanged(new Vector2(Position.X, 0f)));
        }

        private static float KeepXWithinBounds(float x)
        {
            return MathHelper.Clamp(x, -3200f, 400f);
        }

        private float KeepYWithinBounds(float y)
        {
            var halfHeight = _camera.GetHeightFromZ() * 0.5f;
            return MathHelper.Clamp(y, -halfHeight * 0.68f, halfHeight * 0.2f);
        }

        private static bool CheckValidateMoveOn(
            Vector2 currentPosition,
            Vector2 destinationPosition,
            DirectionType direction)
        {
            // if this location is invalid regarding on some measuring and validations, as a result probably user are cheating,
            // we must send an event to server or anti-cheat for doing some actions later

            return true;
        }
    }
}