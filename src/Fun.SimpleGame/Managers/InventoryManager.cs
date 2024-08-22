using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Fun.Engine.Physics;
using Fun.SimpleGame.Entities;
using Fun.SimpleGame.UI.HUDWidgets;

namespace Fun.SimpleGame.Managers
{
    public interface IInventoryManager
    {
        void Add(Collectible collectible);

        void AddRange(IEnumerable<Collectible> collectibles);

        void Update(GameTime gameTime);

        void Collect(PlayerSprite player);

        void IncreaseHealth(PlayerSprite player);

        void ApplyShield(PlayerSprite player);

        IEnumerable<KeyValuePair<AssetType, int>> GetAssets();//Retrieve
    }

    public class InventoryManager : IInventoryManager
    {
        private readonly Dictionary<AssetType, int> _collectedObjects = [];
        private readonly List<Collectible> _collectibles = [];
        private readonly object _locker = new();

        public bool HasChanges { get; private set; }

        public void Add(Collectible collectible)
        {
            lock (_locker)
            {
                _collectibles.Add(collectible);
            }
        }

        public void AddRange(IEnumerable<Collectible> collectibles)
        {
            lock (_locker)
            {
                _collectibles.AddRange(collectibles);
            }
        }

        public void Update(GameTime gameTime)
        {
            lock (_locker)
            {
                for (var i = 0; i < _collectibles.Count; i++)
                {
                    _collectibles[i].Update(gameTime);
                }
            }
        }

        public void Collect(PlayerSprite player)
        {
            lock (_locker)
            {
                for (var i = 0; i < _collectibles.Count; i++)
                {
                    if (DetectCollision(_collectibles[i].CollisionBoxes, _collectibles[i].ScreenPosition,
                            player.CollisionBoxes, player.ScreenPosition))
                    {
                        if (_collectedObjects.ContainsKey(_collectibles[i].Type))
                        {
                            _collectedObjects[_collectibles[i].Type] += _collectibles[i].Amount;
                        }
                        else
                        {
                            _collectedObjects.Add(_collectibles[i].Type, _collectibles[i].Amount);
                        }

                        _collectibles[i].Remove();

                        _collectibles.RemoveAt(i);

                        HasChanges = true;

                        break;
                    }
                }
            }
        }

        public void IncreaseHealth(PlayerSprite player)
        {
            lock (_locker)
            {
                if (_collectedObjects.TryGetValue(AssetType.Health, out var value3) && value3 > 0)
                {
                    _collectedObjects[AssetType.Health]--;
                    return;
                }

                if (_collectedObjects.TryGetValue(AssetType.Yellow, out var value1) && value1 >= 10 &&
                    _collectedObjects.TryGetValue(AssetType.Green, out var value2) && value2 >= 4)
                {
                    value1 -= 10;
                    value2 -= 4;
                    _collectedObjects[AssetType.Yellow] = value1;
                    _collectedObjects[AssetType.Green] = value2;
                }
                //else
                //{
                //    //return a message contains you don't have enough assets
                //}
            }
        }

        public void ApplyShield(PlayerSprite player)
        {
            //based on written rules
            lock (_locker)
            {

            }
        }

        public IEnumerable<KeyValuePair<AssetType, int>> GetAssets()
        {
            //return all collectible objects that are collected
            lock (_locker)
            {
                HasChanges = false;

                return _collectedObjects.AsEnumerable();
            }
        }

        private static bool DetectCollision(IEnumerable<CollisionBox2D> a, Vector2 aS, IEnumerable<CollisionBox2D> b, Vector2 bS)
        {
            // currently, this code works well based on x-axis of ScreenPosition, but it should be refactored
            var aX = aS.X;
            var bX = bS.X;
            foreach (var collisionBox2D in a)
            {
                foreach (var other in b)
                {
                    if (collisionBox2D.CollidesWith(aX, other, bX))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}