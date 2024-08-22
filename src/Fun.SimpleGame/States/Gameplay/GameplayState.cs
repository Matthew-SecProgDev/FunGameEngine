using System;
using System.Collections.Generic;
using Fun.Engine;
using Fun.Engine.Audio;
using Fun.Engine.Audio.Models;
using Fun.Engine.Events;
using Fun.Engine.Events.Interfaces;
using Fun.Engine.Physics;
using Fun.Engine.States;
using Fun.SimpleGame.Entities;
using Fun.SimpleGame.Managers;
using Fun.SimpleGame.Managers.CollisionRules;
using Fun.SimpleGame.Managers.Interfaces;
using Fun.SimpleGame.UI.HUDWidgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RNG = Fun.Engine.Utilities.RandomNumberGenerator;

namespace Fun.SimpleGame.States.Gameplay
{
    public partial class GameplayState : BaseGameState
    {
        private readonly BackgroundMusicManager _backgroundMusicManager;
        private readonly ICollisionManager _collisionManager;
        private readonly IEventBus _eventBus;

        private IEnemyManager _enemyManager;
        private PlayerSprite _player;

        private readonly IInventoryManager _inventoryManager;
        private HeadsUpDisplay _headsUpDisplay;

        public GameplayState() : base(new GameplayInputMapper())
        {
            _backgroundMusicManager = new BackgroundMusicManager();
            _collisionManager = new CollisionManager();
            _eventBus = new EventBus();
            _inventoryManager = new InventoryManager();

            //IsDebugCollisionEnabled = true;
        }

        public override void LoadContent()
        {
            InitializeTerrainBackground();
            InitializePlayer();

            AddInventories();

            _headsUpDisplay = new HeadsUpDisplay(Shapes, Camera, _player, AddObject, 
                ResourceManager, _inventoryManager, _eventBus);
            AddObject(_headsUpDisplay);            

            InitializeEnemyManager();
            InitializeCollisionRules();

            const string music1 = "Audio/Music/battle_music";
            const string music2 = "Audio/Music/battle_music2";
            var audioSettings = new AudioSettings(0.3f, 0f, 0f);
            _backgroundMusicManager.SetBackgroundMusic([
                new AudioTrack(ResourceManager.LoadSound(music1), audioSettings),
                new AudioTrack(ResourceManager.LoadSound(music2), audioSettings)
            ]);

            InitializePlayerEventSubscribers();
            InitializeTerrainBackgroundEventSubscribers();
        }

        public override void HandleInput(GameTime gameTime)
        {
            InputManager.Update();
            InputManager.UpdateMouseWorldPosition(Screen, Camera);

            //var gameEngineMouse = Engine.Input.Mouse.Instance;
            //gameEngineMouse.Update();
            //if (Engine.Input.Keyboard.Instance.IsKeyPressed(Keys.F))
            //{
            //    Camera.GetExtents(out var left, out var right, out var bottom, out var top);
            //    Console.WriteLine("right:  " + right + "   left:  " + left);
            //    Console.WriteLine("top:  " + top + "   bottom:  " + bottom);

            //    //Console.WriteLine("Player Position: " + _playerSprite.Position);

            //    //Console.WriteLine("Screen Position: " + _terrainBackgroundSprite.Position);

            //    //Console.WriteLine("Mouse Position(old implementation): " + InputManager.GetMousePosition());
            //    Console.WriteLine("MouseScreen Position: " + gameEngineMouse.GetScreenPosition(Screen));
            //    Console.WriteLine("MouseWorld Position: " + gameEngineMouse.GetWorldPosition(Screen, Camera));

            //    Console.WriteLine();
            //}

            var elapsedTime = gameTime.GetElapsedSeconds();
            InputManager.GetCommands(gameTime, cmd =>
            {
                if (cmd is GameplayInputCommand.GameExit)
                {
                    NotifyEvent(new BaseGameStateEvent.GameQuit());
                    return;
                }

                if (!_player.IsAlive)
                {
                    return;
                }

                if (_player.IsFrozen)
                {
                    return;
                }

                switch (cmd)
                {
                    case GameplayInputCommand.PlayerPickUps:
                        _inventoryManager.Collect(_player);
                        break;

                    case GameplayInputCommand.PlayerMoveLeft:
                        _player.TryMoveLeft(elapsedTime);
                        break;

                    case GameplayInputCommand.PlayerMoveRight:
                        _player.TryMoveRight(elapsedTime);
                        break;

                    case GameplayInputCommand.PlayerMoveUp:
                        _player.TryMoveUp(elapsedTime);
                        break;

                    case GameplayInputCommand.PlayerMoveDown:
                        _player.TryMoveDown(elapsedTime);
                        break;

                    case GameplayInputCommand.PlayerStopsMoving:
                        _player.StopMoving();
                        break;

                    case GameplayInputCommand.PlayerAttack attack:
                        _player.PerformAttack(gameTime, attack.Type);
                        break;
                }
            });
        }

        protected override void UpdateState(GameTime gameTime)
        {
            _inventoryManager.Update(gameTime);

            _headsUpDisplay.Update(gameTime);

            Camera._screenShake.Update(gameTime);

            _player.Update(gameTime);

            _enemyManager.Update(gameTime);

            _collisionManager.Update();

            //_backgroundMusicManager.PlayMusic();
        }

        private void InitializeTerrainBackground()
        {
            var terrainBackground = new TerrainBackgroundSprite(ResourceManager, _eventBus, Camera)
            {
                ZIndex = -100
            };

            _eventBus.Subscribe<GameplayEvents.GoingToLocation>(terrainBackground.HandleMoving);

            AddObject(terrainBackground);
        }

        private void InitializePlayer()
        {
            // it must be refactored
            Camera.GetExtents(out float width, out float height);
            var halfHeight = height * 0.5f;
            var playerPosY = -200;//RNG.NextRandom(-halfHeight * 0.60f, halfHeight * 0.10f);
            var playerPosX = -(width * 0.5f) * 0.85f;
            _player = new PlayerSprite(ResourceManager, _eventBus)
            {
                Position = new Vector2(playerPosX, playerPosY),
                MovementDirection = Vector2.UnitX,
                Height = 128,
                Width = 128
            };
            _player.AddObjectHandler += AddObject;//it must be unsubscribed
            _player.RemoveObjectHandler += RemoveObject;//it must be unsubscribed

            AddObject(_player);

            //NotifyEvent(new GameplayEvents.PlayerDirectionChanged(direction));
        }

        private void AddInventories()
        {
            var collectibles = new Collectible[]
            {
                new(ResourceManager, "Textures/PickUps/41")
                {
                    Position = new Vector2(100, 150),
                    Type = AssetType.Yellow,
                    Amount = 20,
                    Width = 40,
                    Height = 40
                },
                new(ResourceManager, "Textures/PickUps/42")
                {
                    Position = new Vector2(100, 50),
                    Type = AssetType.Green,
                    Amount = 10,
                    Width = 40,
                    Height = 40
                },
                new(ResourceManager, "Textures/PickUps/43")
                {
                    Position = new Vector2(100, -100),
                    Type = AssetType.Blue,
                    Amount = 50,
                    Width = 40,
                    Height = 40
                },
                new(ResourceManager, "Textures/PickUps/44")
                {
                    Position = new Vector2(80, -200),
                    Type = AssetType.Red,
                    Amount = 100,
                    Width = 40,
                    Height = 40
                },
                new(ResourceManager, "Textures/PickUps/45")
                {
                    Position = new Vector2(150, -250),
                    Type = AssetType.Coin,
                    Amount = 35,//randomly between 35 and 50
                    Width = 40,
                    Height = 40
                }
            };

            foreach (var collectible in collectibles)
            {
                collectible.AddCollisionBox(new CollisionBox2D(collectible.Position, collectible.Width, collectible.Height));
                collectible.RemoveObjectHandler += RemoveObject;//it must be unsubscribed
                AddObject(collectible);
            }

            _inventoryManager.AddRange(collectibles);
        }

        private void InitializeEnemyManager()
        {
            _enemyManager = new EnemyManager(Camera, ResourceManager, _eventBus);
            _enemyManager.AddObjectHandler += AddObject;
            _enemyManager.RemoveObjectHandler += RemoveObject;
            _enemyManager.Initialize(_player.Position);
        }

        private void InitializeCollisionRules()
        {
            _collisionManager.AddCollisionRule(new HitPlayerByEnemiesAttacks(_eventBus, _enemyManager.EnemyHitboxes, _player));
            _collisionManager.AddCollisionRule(new HitEnemiesByPlayerAttacks(_player.Hitboxes, _enemyManager.Enemies));
            _collisionManager.AddCollisionRule(new HitEnemiesByPlayerShots(_player.Arrows, _enemyManager.Enemies));
            //_collisionManager.AddCollisionRule(new BlockPlayerMovementOnCollisionWithEnemy(_enemyManager.EnemiesList, _playerSprite));
        }

        private void ScreenShake(float duration = 0.4f, float magnitude = 4f)
        {
            // we later can get duration and magnitude based on each weapon
            Camera.ApplyShake(duration, magnitude);
        }
    }
}