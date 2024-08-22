using Microsoft.Xna.Framework;

namespace Fun.Engine.UI.Icons
{
    public class Icon : BaseUIObject
    {
        public Icon(Vector2 size, Models.IconOption option, Vector2 position) : base(size, position)
        {
            Texture = option.Texture;
        }

        public override void Render(Graphics.Sprites sprites)
        {
            var destination = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

            sprites.Draw(Texture, destination, null, Color.White);
        }
    }
}