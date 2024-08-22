using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Fun.SimpleGame.Managers;
using Fun.Engine.UI;
using Fun.Engine.UI.Buttons;
using Fun.Engine.UI.Buttons.Models;
using Fun.Engine.UI.Icons;
using Fun.Engine.UI.Icons.Models;
using Fun.Engine.Resources;
using Fun.Engine.Geometry;
using Fun.SimpleGame.UI.HUDWidgets.Models;
using System.IO;
using Fun.Engine.Graphics;
using Fun.SimpleGame.States.Gameplay;
using Fun.Engine.Events.Interfaces;
using Fun.SimpleGame.Entities;
using XnaMathHelper = Microsoft.Xna.Framework.MathHelper;
using System.Xml.Linq;

namespace Fun.SimpleGame.UI.HUDWidgets
{
    public class HeadsUpDisplay : Engine.Entities.BaseObject
    {
        private readonly Shapes _shapes;
        private readonly Camera _camera;
        private readonly Vector2[] _frameVertices;
        private readonly int[] _frameIndices;
        private readonly Transform2D _frameTransform;
        private readonly Color _frameColor;

        private readonly IInventoryManager _inventoryManager;
        private readonly PlayerSprite _player;
        private readonly SpriteFont _font;
        private readonly Color _penColor;

        private readonly Dictionary<AssetType, BaseUIObject> _elements;
        private readonly List<BaseUIObject> _otherElements = [];

        public HeadsUpDisplay(
            Shapes shapes,
            Camera camera,
            PlayerSprite player,
            Action<Engine.Entities.BaseObject> addObjectHandler,
            IResourceManager resourceManager,
            IInventoryManager inventoryManager,
            IEventBus eventBus)
        {
            ArgumentNullException.ThrowIfNull(shapes);
            ArgumentNullException.ThrowIfNull(camera);
            ArgumentNullException.ThrowIfNull(player);

            _shapes = shapes;
            _camera = camera;
            _player = player;
            _inventoryManager = inventoryManager;
            _font = resourceManager.LoadFont("Fonts/font");
            _penColor = Color.White;

            //Initialize the frame
            InitializeFrame(out _frameVertices, out _frameIndices, out _frameTransform, out _frameColor);

            //Initialize elements of the frame
            InitializeFrameElements(addObjectHandler, resourceManager, out _elements);

            //Initialize player's health-bar
            camera.GetExtents(out float width, out float height);
            InitializePlayerHealthBar(width, height, addObjectHandler, resourceManager, eventBus);

            //Initialize enemy's health-bar
            InitializeEnemyHealthBar(width, height, addObjectHandler, resourceManager, eventBus);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var element in _otherElements)
            {
                element.Update(gameTime);
            }

            foreach (var element in _elements.Values)
            {
                element.Update(gameTime);
            }
        }

        public override void Render(Engine.Graphics.Sprites sprites)
        {
            _shapes.Begin(_camera);
            _shapes.DrawFilledPolygon(_frameVertices, _frameIndices, _frameTransform, _frameColor, 0.6f);
            _shapes.End();

            //var j = 0;
            const float scale = 1.3f;
            const float width = 40f;
            const float height = 40f;
            foreach (var (key, value) in _inventoryManager.GetAssets())
            {
                if (!_elements.TryGetValue(key, out var @object))
                {
                    //log
                    continue;
                }

                var text = value.ToString();
                var textSize = _font.MeasureString(text) * scale;

                var x = @object.Position.X + (width / 2f) - (textSize.X / 2f);
                var y = @object.Position.Y + height + 5f;
                sprites.DrawString(_font, text, new Vector2(x, y), _penColor, Vector2.Zero,
                    SpriteEffects.FlipVertically, scale: scale);
            }
        }

        private void InitializeFrameElements(
            Action<Engine.Entities.BaseObject> addObjectHandler,
            IResourceManager resourceManager,
            out Dictionary<AssetType, BaseUIObject> elements)
        {
            var x = -200f;
            var y = -490f;
            var size = new Vector2(40f, 40f);

            elements = new Dictionary<AssetType, BaseUIObject>
            {
                {
                    AssetType.Yellow,
                    new Icon(size,
                        new IconOption { Texture = resourceManager.LoadTexture("Textures/UI/PickUps/41") },
                        new Vector2(x, y))
                },
                {
                    AssetType.Green,
                    new Icon(size,
                        new IconOption { Texture = resourceManager.LoadTexture("Textures/UI/PickUps/42") },
                        new Vector2(20f + x + size.X, y))
                },
                {
                    AssetType.Blue,
                    new Icon(size,
                        new IconOption { Texture = resourceManager.LoadTexture("Textures/UI/PickUps/43") },
                        new Vector2(20f * 2 + x + size.X * 2, y))
                },
                {
                    AssetType.Red,
                    new Icon(size,
                        new IconOption { Texture = resourceManager.LoadTexture("Textures/UI/PickUps/44") },
                        new Vector2(20f * 3 + x + size.X * 3, y))
                },
                {
                    AssetType.Coin,
                    new Icon(size,
                        new IconOption { Texture = resourceManager.LoadTexture("Textures/UI/PickUps/45") },
                        new Vector2(20f * 4 + x + size.X * 4, y))
                },
                {
                    AssetType.Health,
                    new Button(size,
                        new TexturedButtonOption
                        {
                            NormalTexture = resourceManager.LoadTexture("Textures/UI/PickUps/46"),
                            OnClick = () => _inventoryManager.IncreaseHealth(_player)
                        },
                        new Vector2(20f * 5 + x + size.X * 5, y))
                },
                {
                    AssetType.Shield,
                    new Button(size,
                        new TexturedButtonOption
                        {
                            NormalTexture = resourceManager.LoadTexture("Textures/UI/PickUps/47"),
                            OnClick = () => _inventoryManager.ApplyShield(_player)
                        },
                        new Vector2(20f * 6 + x + size.X * 6, y))
                }
            };

            foreach (var element in elements.Values)
            {
                addObjectHandler(element);
            }
        }

        private static void InitializeFrame(
            out Vector2[] vertices,
            out int[] triangleIndices,
            out Transform2D transform,
            out Color color)
        {
            vertices =
            [
                new Vector2(-200, -350),
                new Vector2(200, -350),
                new Vector2(250, -500),
                new Vector2(210, -470),
                new Vector2(-210, -470),
                new Vector2(-250, -500)
            ];

            var triangleCount = vertices.Length - 2;
            triangleIndices = new int[triangleCount * 3];
            triangleIndices = [0, 1, 3, 1, 2, 3, 0, 3, 4, 0, 4, 5];

            transform = new Transform2D(new Vector2(0f, -40f), XnaMathHelper.TwoPi, 1f);
            color = Color.DarkSlateGray;
        }

        private void InitializePlayerHealthBar(
            float width, 
            float height,
            Action<Engine.Entities.BaseObject> addObjectHandler,
            IResourceManager resourceManager,
            IEventBus eventBus)
        {
            const string basePath = "Textures/UI/HealthBar";
            var option = new HealthBarOption
            {
                BackgroundTexture = resourceManager.LoadTexture(Path.Combine(basePath, "background")),
                FillTexture = resourceManager.LoadTexture(Path.Combine(basePath, "fill"))
            };

            var size = new Vector2(width * 0.222f, height * 0.025f);//refactor it
            var position = Position = new Vector2(-width * 0.45f, height * 0.37f);//refactor it
            var playerHealthBar = new PlayerHealthBar(size, option, _player.Health, position);

            eventBus.Subscribe<GameplayEvents.PlayerHit>(playerHealthBar.OnNotify);

            _otherElements.Add(playerHealthBar);

            addObjectHandler(playerHealthBar);
        }

        private void InitializeEnemyHealthBar(
            float width, 
            float height,
            Action<Engine.Entities.BaseObject> addObjectHandler,
            IResourceManager resourceManager,
            IEventBus eventBus)
        {
            const string basePath = "Textures/UI/HealthBar";
            var option = new HealthBarOption
            {
                BackgroundTexture = resourceManager.LoadTexture(Path.Combine(basePath, "background")),
                FillTexture = resourceManager.LoadTexture(Path.Combine(basePath, "fill"))
            };

            var position = new Vector2(-width * 0.43f, height * 0.34f);//refactor it
            var size = new Vector2(width * 0.2f, height * 0.02f);//refactor it
            var enemyHealthBar = new EnemyHealthBar(size, option, position);
            enemyHealthBar.Deactivate();

            eventBus.Subscribe<GameplayEvents.EnemyHit>(enemyHealthBar.OnNotify);
            eventBus.Subscribe<GameplayEvents.EnemyCreated>(enemyHealthBar.OnNotify);

            _otherElements.Add(enemyHealthBar);

            addObjectHandler(enemyHealthBar);
        }
    }
}