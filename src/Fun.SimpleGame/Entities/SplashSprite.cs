using Fun.Engine.Entities;
using Fun.Engine.Resources;

namespace Fun.SimpleGame.Entities
{
    public class SplashSprite : BaseGameObject
    {
        public SplashSprite(IResourceManager resourceManager)
        {
            const string splashTexture = "Textures/Splash/splash_screen";
            Texture = resourceManager.LoadTexture(splashTexture);
        }
    }
}