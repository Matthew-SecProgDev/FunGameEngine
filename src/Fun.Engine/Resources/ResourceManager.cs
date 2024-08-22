using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.Resources
{
    public class ResourceManager(ContentManager content) : IResourceManager
    {
        public Texture2D LoadTexture(string assetName)
        {
            return content.Load<Texture2D>(assetName);
        }

        public SpriteFont LoadFont(string assetName)
        {
            return content.Load<SpriteFont>(assetName);
        }

        public SoundEffect LoadSound(string assetName)
        {
            return content.Load<SoundEffect>(assetName);
        }

        public Effect LoadEffect(string assetName)
        {
            return content.Load<Effect>(assetName);
        }

        public void Unload()
        {
            content.Unload();
        }
    }
}