using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.UI.Buttons.Models
{
    public abstract class BaseButtonOption
    {
        public required Texture2D NormalTexture { get; set; }

        public required Action OnClick { get; set; }
    }
}