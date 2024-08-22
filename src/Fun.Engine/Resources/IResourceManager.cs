using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.Resources
{
    public interface IResourceManager
    {
        public Texture2D LoadTexture(string assetName);

        public SpriteFont LoadFont(string assetName);

        public SoundEffect LoadSound(string assetName);

        public Effect LoadEffect(string assetName);

        public void Unload();
    }
}