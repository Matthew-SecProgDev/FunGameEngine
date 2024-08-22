using System.IO;
using Fun.Engine.Entities;
using Fun.Engine.Resources;
using Fun.SimpleGame.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.SimpleGame.Particles.Simple
{
    // it must be refactored

    public enum ParticleGeneratorStatus
    {
        NotInitialized = 0,

        Running = 1,

        Completed = 2
    }

    // it must be refactored
    public class SimpleEmitterGenerator
    {
        private const string BasePath = "Textures/Effects/Creation";

        private const int MaxExplosionAge = 240; // 4 seconds at 60 frames per second = 240
        private const int ExplosionActiveLength = 195; // emit particles for 3.2 seconds and let them fade out for 4 seconds

        private readonly Texture2D _texture;
        private EnemyEffect _creationEffect;

        private UpwardSimpleEmitter? _simpleEmitter;
        private bool _isInitialized;

        public event ChildObjectHandler? AddObjectHandler;

        public event ChildObjectHandler? RemoveObjectHandler;

        public ParticleGeneratorStatus Status
        {
            get
            {
                if (_isInitialized == false)
                {
                    return ParticleGeneratorStatus.NotInitialized;
                }

                //refactor it
                return _simpleEmitter == null && !_creationEffect.IsActive ? ParticleGeneratorStatus.Completed : ParticleGeneratorStatus.Running;
            }
        }

        public SimpleEmitterGenerator(IResourceManager resourceManager)
        {
            _texture = resourceManager.LoadTexture(Path.Combine(BasePath, "enemy_creation2"));
            _creationEffect = new EnemyEffect(resourceManager, BasePath, "enemy_creation")
            {
                Width = 300,
                Height = 200
            };
        }

        public void Initialize(Color color1, Vector2 position, Vector2 directionEnemy)
        {
            _simpleEmitter = new UpwardSimpleEmitter(_texture, position, color1);

            HandleAddObject(_simpleEmitter);

            InitializeSprite(position, directionEnemy);

            _isInitialized = true;
        }

        private void InitializeSprite(Vector2 position, Vector2 directionEnemy)
        {
            _creationEffect.AddFrame(0, 0, 128, 128, 0.2f);
            _creationEffect.AddFrame(128, 0, 128, 128, 0.2f);
            _creationEffect.AddFrame(256, 0, 128, 128, 0.2f);
            _creationEffect.AddFrame(384, 0, 128, 128, 0.2f);
            _creationEffect.AddFrame(0, 128, 128, 128, 0.2f);
            _creationEffect.AddFrame(128, 128, 128, 128, 0.2f);
            _creationEffect.AddFrame(256, 128, 128, 128, 0.2f);
            _creationEffect.AddFrame(384, 128, 128, 128, 0.2f);
            _creationEffect.AddFrame(0, 256, 128, 128, 0.2f);
            _creationEffect.AddFrame(128, 256, 128, 128, 0.2f);
            _creationEffect.AddFrame(256, 256, 128, 128, 0.2f);
            _creationEffect.AddFrame(384, 256, 128, 128, 0.2f);
            _creationEffect.AddFrame(0, 384, 128, 128, 0.2f);
            _creationEffect.AddFrame(128, 384, 128, 128, 0.2f);
            _creationEffect.AddFrame(256, 384, 128, 128, 0.2f);
            _creationEffect.AddFrame(384, 384, 128, 128, 0.2f);

            var value = directionEnemy == Vector2.UnitX ? 145 : 125;
            _creationEffect.SetPosition((int)position.X - value, (int)position.Y - 150);

            HandleAddObject(_creationEffect);
        }

        public void Update(GameTime gameTime)
        {
            if (_simpleEmitter != null)
            {
                if (_simpleEmitter.Age > ExplosionActiveLength)
                {
                    _simpleEmitter.Deactivate();
                }

                if (_simpleEmitter.Age > MaxExplosionAge)
                {
                    HandleRemoveObject(_simpleEmitter);
                    _creationEffect.Clear();
                    HandleRemoveObject(_creationEffect);
                    _simpleEmitter = null;
                    return;
                }

                _simpleEmitter.Update(gameTime);
                _creationEffect.Update(gameTime);
            }
        }

        private void HandleAddObject(BaseObject baseObject)
        {
            AddObjectHandler?.Invoke(baseObject);
        }

        private void HandleRemoveObject(BaseObject baseObject)
        {
            RemoveObjectHandler?.Invoke(baseObject);
        }
    }
}